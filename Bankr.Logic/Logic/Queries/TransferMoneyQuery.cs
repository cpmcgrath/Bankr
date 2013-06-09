using System;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using CMcG.Bankr.Data;
using System.Collections.Generic;

namespace CMcG.Bankr.Logic.Queries
{
    public class TransferMoneyQuery : CommBankQuery
    {
         public bool    IsValidate    { get; set; }
         public int     FromAccountId { get; set; }
         public int     ToAccountId   { get; set; }
         public string  Description   { get; set; }
         public decimal Amount        { get; set; }

        public string ReceiptNo { get; set; }

        protected override string Action
        {
            get { return "Transfering money..."; }
        }

        protected override NameValue[] Parameters
        {
            get
            {
                return new[]
                {
                    new NameValue("Request",       IsValidate ? "validateTransfer" : "processTransfer"),
                    new NameValue("AccountFromId", FromAccountId),
                    new NameValue("AccountToId",   ToAccountId),
                    new NameValue("Description",   Description ?? string.Empty),
                    new NameValue("Amount",        Amount.ToString("0.00")),
                };
            }
        }

        protected override void ProcessResult(string response, bool hasError)
        {
            if (hasError)
                Status.SetAction("Error occurred during transfer.", true);

            if (IsValidate || hasError)
                return;

            ReceiptNo = Newtonsoft.Json.Linq.JObject.Parse(response).Value<string>("ReceiptNo");
            Status.SetAction("Money sent.", true);
        }

        bool AlreadyExists(IEnumerable<UpcomingTransaction> list, UpcomingTransaction transaction)
        {
            return list.Any(y => y.Equals(transaction));
        }
    }
}
