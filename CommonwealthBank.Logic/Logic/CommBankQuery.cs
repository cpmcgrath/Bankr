using System;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using CMcG.CommonwealthBank.Data;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

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

        public Uri RequestUri
        {
            get { return new Uri("mobile/i/AjaxCalls.aspx?SID=" + SessionId, UriKind.Relative); }
        }

        public HttpContent GetContent()
        {
            var    parameters   = new JsonParameters { Params = Parameters };
            string paramRequest = JsonConvert.SerializeObject(parameters);

            var stringContent = new StringContent(paramRequest);
            stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            return stringContent;
        }

        public async void Start(HttpClient client)
        {
            if (!CanRun())
                return;

            Status.SetAction(Action);

            try
            {
                var response = await client.PostAsync(RequestUri, GetContent());
                var content  = await response.Content.ReadAsStringAsync();
                ProcessResult(client, content);
            }
            catch (WebException)
            {
                Status.SetAction("Cannot find the server", true);
                Callback();
            }
        }

        void ProcessResult(HttpClient client, string result)
        {
            var response = result.Substring(2, result.Length - 4);
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