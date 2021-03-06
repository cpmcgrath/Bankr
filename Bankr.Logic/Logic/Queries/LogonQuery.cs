﻿using System;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using CMcG.Bankr.Data;
using System.Collections.Generic;

namespace CMcG.Bankr.Logic.Queries
{
    public class LogonQuery : CommBankQuery
    {
        public static LoginDetails GetLogonDetails()
        {
            using (var store = new DataStoreContext())
                return store.LoginDetails.FirstOrDefault();
        }

        protected override string Action
        {
            get { return "Logging on..."; }
        }

        protected override NameValue[] Parameters
        {
            get
            {
                var loginDetails = GetLogonDetails();
                var username     = loginDetails.Username;
                var password     = new TwoWayEncryption().Decrypt(loginDetails.Password);

                return new[]
                {
                    new NameValue("Request",  "login"),
                    new NameValue("UserName", username),
                    new NameValue("Password", password),
                    new NameValue("Token",    "")
                };
            }
        }

        protected override void ProcessResult(string response, bool hasError)
        {
            if (hasError)
                OnLogonFailed();
            else
                UpdateAccounts(response);
        }

        void OnLogonFailed()
        {
            using (var store = new DataStoreContext())
            {
                store.LoginDetails.DeleteAllOnSubmit(store.LoginDetails);
                store.SubmitChanges();
            }
            Status.SetAction("Error occured while trying to log on", true);
        }

        void UpdateAccounts(string message)
        {
            var result = JsonConvert.DeserializeObject<LoginResult>(message);

            using (var store = new DataStoreContext())
            {
                foreach (var account in result.AccountGroups.SelectMany(x => x.ListAccount))
                {
                    account.LastUpdate = DateTime.Now;
                    var original = store.Accounts.FirstOrDefault(x => x.AccountNumber == account.AccountNumber);
                    if (original == null)
                        store.Accounts.InsertOnSubmit(account);
                    else
                    {
                        original.Balance        = account.Balance;
                        original.AvailableFunds = account.AvailableFunds;
                        original.LastUpdate     = account.LastUpdate;
                    }
                }

                store.SubmitChanges();
            }

            SessionId = result.SID;
        }
    }
}