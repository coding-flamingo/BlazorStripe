using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using StripeProject.Client.Components;
using StripeProject.Shared;

namespace StripeProject.Client.Pages
{
    public class PaymentPageBase : ComponentBase
    {
        protected StripeComponent StripePaymentBase;
        [Inject] NavigationManager _navigationManager { get; set; }
        [Inject] HttpClient _httpClient { get; set; }
        protected StripeBillingRequest BillingInfo;
        protected override void OnInitialized()
        {
            BillingInfo = new();
            base.OnInitialized();
        }

        protected async Task SendSubToServerAsync(bool randy)
        {
            var result = await PostToBackend(_navigationManager.BaseUri + "api/Stripe/ActivateSubscription",
                JsonSerializer.Serialize(BillingInfo));
            if (result.Success)
            {
                APIResultModel apiResult = JsonSerializer.Deserialize<APIResultModel>(result.Message);
                if (apiResult.Success)
                {
                   //success send the customer to the app
                }
                else
                {
                    //error creating subscription 
                }
            }
            else
            {
                //error contacting server
            }
        }

        private async Task<APIResultModel> PostToBackend(string url, string jsonPayload)
        {
            APIResultModel apiResult = new APIResultModel();
            HttpResponseMessage responseMessage;
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            try
            {
                requestMessage.Content = new StringContent(jsonPayload,
                    Encoding.UTF8, "application/json");
                responseMessage = await SendMessageAsync(requestMessage);
                apiResult.Message = await responseMessage.Content.ReadAsStringAsync();
                apiResult.Success = responseMessage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                apiResult.Success = false;
                if (ex.Message.Contains("One or more errors"))
                {
                    apiResult.Message = ex.InnerException.Message;
                }
                else if (ex.Message.Equals("The request message was already sent. Cannot send the same request message multiple times."))
                {
                    apiResult.Message = "Error contacting Server Please try again later";
                }
                else
                {
                    apiResult.Message = ex.Message;
                }
            }
            return apiResult;
        }

        private async Task<HttpResponseMessage> SendMessageAsync(HttpRequestMessage requestMessage)
        {
            HttpResponseMessage response;
            response = await _httpClient.SendAsync(requestMessage);
            return response;
        }
    }
}
