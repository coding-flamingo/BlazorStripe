using System;
using System.Collections.Generic;
using System.Linq;
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
        protected StripeBillingRequest BillingInfo;
        protected override void OnInitialized()
        {
            BillingInfo = new();
            base.OnInitialized();
        }

        protected async Task SendSubToServerAsync(bool randy)
        {
            //todo Subscribe to see what happens next
        }
    }
}
