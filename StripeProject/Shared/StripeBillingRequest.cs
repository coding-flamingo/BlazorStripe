using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StripeProject.Shared
{
    public class StripeBillingRequest
    {
        [JsonPropertyName("BillingName")]
        [Required(ErrorMessage = "Please enter cardholders full name")]
        public string BillingName { get; set; }
        [JsonPropertyName("BillingEmail")]
        [Required(ErrorMessage = "Please enter a valid email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string BillingEmail { get; set; }
        [JsonPropertyName("paymentMethodId")]
        public string PaymentMethod { get; set; }
        [JsonPropertyName("priceId")]
        public string Price { get; set; }
    }
}
