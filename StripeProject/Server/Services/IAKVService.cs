using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace StripeProject.Server.Services
{
    public class AKVService : IAKVService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly SecretClient _akvSecretClient;
        private readonly string _akvURL;
        public AKVService(IMemoryCache memoryCache,
            IConfiguration configuration)
        {
            _akvURL = configuration.GetSection("KeyVaults").GetValue<string>("MainAKV");
            _akvSecretClient = new SecretClient(new Uri(_akvURL), new DefaultAzureCredential());
            _memoryCache = memoryCache;
        }

        public async Task<string> GetKeyVaultSecretAsync(string secretName)
        {
            string secret;
            if (!_memoryCache.TryGetValue(secretName, out secret))
            {
                KeyVaultSecret returnedSecret = await _akvSecretClient.GetSecretAsync(secretName);
                secret = returnedSecret.Value;
                _memoryCache.Set(secretName, secret, DateTimeOffset.UtcNow.AddHours(1));
            }
            return secret;
        }

        public async Task<X509Certificate2> GetKeyVaultCertAsync(string secretName)
        {
            string secret = await GetKeyVaultSecretAsync(secretName);
            X509Certificate2 cert = new X509Certificate2(Convert.FromBase64String(secret));
            return cert;
        }
    }
    public interface IAKVService
    {
        Task<string> GetKeyVaultSecretAsync(string secretName);
        Task<X509Certificate2> GetKeyVaultCertAsync(string secretName);
    }
}
