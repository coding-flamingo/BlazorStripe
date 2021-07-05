using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using StripeProject.Shared;

namespace StripeProject.Server.Models
{
    public class DBStripeCustomerModel
    {
        public DBStripeCustomerModel()
        {

        }

        public DBStripeCustomerModel(StripeBillingRequest subReq, string clientID)
        {
            ClientID = clientID;
            BillingName = subReq.BillingName;
            BillingEmail = subReq.BillingEmail;
            PaymentMethod = subReq.PaymentMethod;
        }


        [Key] [Required] public string ClientID { get; set; }
        [Required] public string BillingName { get; set; }
        [Required] public string BillingEmail { get; set; }
        [Required] public string PaymentMethod { get; set; }

    }
}
