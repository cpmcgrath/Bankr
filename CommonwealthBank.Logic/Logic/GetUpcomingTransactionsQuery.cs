using System;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using CMcG.CommonwealthBank.Data;
using System.Collections.Generic;

namespace CMcG.CommonwealthBank.Logic
{
    public class GetUpcomingTransactionsQuery : CommBankQuery
    {
        int m_accountId;

        protected override string Action
        {
            get { return "Looking for upcoming transactions..."; }
        }

        protected override NameValue[] Parameters
        {
            get
            {
                return new[] { new NameValue("Request", "getFutureTransactions"), };
            }
        }

        protected override void OnCompleted(WebClient client, string response, bool hasError)
        {
            var result = JsonConvert.DeserializeObject<UpcomingTransResult>(response);

            using (var store = new DataStoreContext())
            {
                var toDelete = store.UpcomingTransactions.ToArray().Where(x => !AlreadyExists(result.Transactions, x));
                var toInsert = result.Transactions.Where(x => !AlreadyExists(store.UpcomingTransactions, x)).ToArray();

                store.UpcomingTransactions.DeleteAllOnSubmit(toDelete);
                store.UpcomingTransactions.InsertAllOnSubmit(toInsert);

                store.SubmitChanges();
                Status.SetAction("Upcoming transactions found.", true);
            }

            Callback();
        }

        bool AlreadyExists(IEnumerable<UpcomingTransaction> list, UpcomingTransaction transaction)
        {
            return list.Any(y => y.Equals(transaction));
        }

        /*{
         * "EnableLogging":false,
         * "ErrorMessages":[],
         * "IsSessionTimeout":false,
         * "IsUnsafeActionDetected":false,
         * "SID":"Rkw3k3WQgng%3d",
         * "Switches":[{"IsEnabled":false,
         *              "Name":"CardActivation"
         *             },
         *             {"IsEnabled":false,
         *              "Name":"CCOffers"
         *             }],
         * "WarningMessages":[],
         * "Transactions":
         * [
         *     {
         *         "EffectiveDate":"03\/04\/2013",
         *         "TransactionType":"3<sup>rd<\/sup> Party Transfer"
         *         "Status":"Pending",
         *         "Amount":"$21.00 ",
         *         "TransactionFrom":"418610178402",
         *         "TransactionTo":"Liam T Moss",
         *     },
         *     {
         *         "Amount":"$245.00 ","EffectiveDate":"05\/04\/2013","Status":"Pending","TransactionFrom":"418610178402","TransactionTo":"Rental Agency","TransactionType":"Bill Payment"}]}*/
    }
}
