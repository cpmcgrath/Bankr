using System;
using System.Linq;
using Newtonsoft.Json;
using CMcG.Bankr.Data;
using System.Collections.Generic;

namespace CMcG.Bankr.Logic.Queries
{
    public class GetTransferAccountsQuery : CommBankQuery
    {
        protected override string Action
        {
            get { return "Looking for transfer accounts..."; }
        }

        protected override NameValue[] Parameters
        {
            get { return new[] { new NameValue("Request", "initTransfer") }; }
        }

        protected override void ProcessResult(string response, bool hasError)
        {
            var result = JsonConvert.DeserializeObject<TransferAccountResult>(response);
            foreach (var account in result.AccountsToLinked)
            {
                account.Type     = TransferToAccount.AccountType.Linked;
                account.SenderId = result.AccountsFrom.Where(x => x.AccountNumber == account.AccountNumber).Select(x => x.Id).FirstOrDefault();
            }

            foreach (var account in result.AccountsToNotLinked)
                account.Type = TransferToAccount.AccountType.ThirdParty;

            var accounts = result.AccountsToLinked.Union(result.AccountsToNotLinked);

            foreach (var account in accounts.Where(x => x.LastUsed == DateTime.MinValue))
                account.LastUsed = new DateTime(2000, 1, 1);

            using (var store = new DataStoreContext())
            {
                var inDatabase = store.TransferToAccounts.ToArray();
                var toDelete   = inDatabase.Where(x => !AlreadyExists(accounts, x)).ToArray();
                var toInsert   = accounts.Where(x => !AlreadyExists(inDatabase, x)).ToArray();

                store.TransferToAccounts.DeleteAllOnSubmit(toDelete);
                store.TransferToAccounts.InsertAllOnSubmit(toInsert);

                store.SubmitChanges();
                Status.SetAction("Accounts loaded", true);
            }
        }

        bool AlreadyExists(IEnumerable<TransferToAccount> list, TransferToAccount transaction)
        {
            return list.Any(y => y.Equals(transaction));
        }
    }
}
