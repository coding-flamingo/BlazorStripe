using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StripeProject.Shared;

namespace StripeProject.Client.Components
{
    public partial class StripeComponent : IDisposable
    {
        [Inject] IJSRuntime _js { get; set; }
        [Parameter] public StripeBillingRequest _subRequest { get; set; }
        [Parameter] public EventCallback<bool> PaymentProcessed { get; set; }
        protected bool _firstTime;
        private DotNetObjectReference<StripeComponent> _objRef;
        protected override async Task OnInitializedAsync()
        {
            _firstTime = true;
        }

        public void Dispose()
        {
            _objRef?.Dispose();
        }

        

        public async Task ProcessPaymentAsync()
        {
            _objRef = DotNetObjectReference.Create(this);
            await _js.InvokeVoidAsync("createPaymentMethodServer", _objRef, _subRequest.BillingEmail, _subRequest.BillingName);
        }

     
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (_firstTime)
            {
                _firstTime = false;
                await _js.InvokeVoidAsync("Initiate");
            }
        }

        [JSInvokable("Subscribe")]
        public Task Subscribe(string paymentID)
        {
            _subRequest.PaymentMethod = paymentID;
            return PaymentProcessed.InvokeAsync(true);
        }
    }
}
