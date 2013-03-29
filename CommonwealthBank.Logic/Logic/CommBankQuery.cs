using System;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CMcG.CommonwealthBank.Logic
{
    public abstract class CommBankQuery
    {
        public AppStatus Status    { get; set; }
        public string    SessionId { get; set; }

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

        public async Task<string> Start(HttpClient client)
        {
            if (!CanRun())
                return null;

            Status.SetAction(Action);

            try
            {
                var response = await client.PostAsync(RequestUri, GetContent());
                var content  = await response.Content.ReadAsStringAsync();
                ProcessResult(client, content);
                return SessionId;
            }
            catch (WebException)
            {
                Status.SetAction("Cannot find the server", true);
                return null;
            }
        }

        void ProcessResult(HttpClient client, string result)
        {
            var response = result.Substring(2, result.Length - 4);
            var hasError = Newtonsoft.Json.Linq.JObject.Parse(response)["ErrorMessages"].Any();
            OnCompleted(response, hasError);
        }

        protected abstract void OnCompleted(string response, bool hasError);
    }
}