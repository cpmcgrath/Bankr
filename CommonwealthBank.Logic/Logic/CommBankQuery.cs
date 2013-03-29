using System;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using CMcG.CommonwealthBank.Data;
using System.Collections.Generic;

namespace CMcG.CommonwealthBank.Logic
{
    public abstract class CommBankQuery
    {
        public AppStatus     Status    { get; set; }
        public Action        Callback  { get; set; }
        public CommBankQuery Next      { get; set; }
        public string        SessionId { get; set; }

        protected abstract string      Action     { get; }
        protected abstract NameValue[] Parameters { get; }

        protected virtual bool CanRun()
        {
            return true;
        }

        public void Start(WebClient client)
        {
            if (!CanRun())
                return;

            Status.SetAction(Action);
            var    parameters   = new JsonParameters { Params = Parameters };
            string paramRequest = JsonConvert.SerializeObject(parameters);

            client.Headers["Content-Type"] = "application/json";
            client.UploadStringCompleted += OnCompleted;
            client.UploadStringAsync(new Uri("mobile/i/AjaxCalls.aspx?SID=" + SessionId, UriKind.Relative), "POST", paramRequest);
        }

        void OnCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            var client = (WebClient)sender;
            client.UploadStringCompleted -= OnCompleted;
            if (e.Error is WebException)
            {
                Status.SetAction("Cannot find the server", true);
                Callback();
                return;
            }

            var response = e.Result.Substring(2, e.Result.Length - 4);
            var hasError = Newtonsoft.Json.Linq.JObject.Parse(response)["ErrorMessages"].Any();
            OnCompleted(response, hasError);

            if (Next != null)
            {
                Next.Status    = Status;
                Next.SessionId = SessionId;
                Next.Callback  = Callback;
                Next.Start(client);
            }
        }

        protected abstract void OnCompleted(string response, bool hasError);
    }
}