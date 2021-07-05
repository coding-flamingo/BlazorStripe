using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StripeProject.Server.Managers;
using StripeProject.Shared;

namespace StripeProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly StripeManager _stripeMan;
        public StripeController(ILogger<StripeController> logger, StripeManager stripeManager)
        {
            _logger = logger;
            _stripeMan = stripeManager;
        }
        [HttpPost("ActivateSubscription")]
        public async Task<APIResultModel> ActivateSubscriptionAsync(StripeBillingRequest subReq)
        {
            APIResultModel result = _stripeMan.VerifySubRequest(subReq);
            if (result.Success == false)
            {
                return result;
            }
            try
            {
                return await _stripeMan.ActivateSubscriptionAsync(subReq);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating Stripe Sub");
                result.Success = false;
                result.Message = "Error activating your subscription, please try again later. If the problem persists, please contact our support team.";
            }
            return result;
        }
    }
}
