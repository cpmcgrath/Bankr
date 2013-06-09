using System;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using CMcG.Bankr.Data;
using System.Collections.Generic;

namespace CMcG.Bankr.Logic.Queries
{
    public class GetUpcomingTransactionsQuery : CommBankQuery
    {
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

        protected override void ProcessResult(string response, bool hasError)
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
        }

        bool AlreadyExists(IEnumerable<UpcomingTransaction> list, UpcomingTransaction transaction)
        {
            return list.Any(y => y.Equals(transaction));
        }
    }
}
