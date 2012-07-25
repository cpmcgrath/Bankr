using System;
using System.Net;
using System.Linq;
using System.Security;

namespace CMcG.CommonwealthBank.Logic
{
     public class CookieAwareWebClient : WebClient
     {
        [SecuritySafeCritical]
         public CookieAwareWebClient()
         {
         }

         CookieContainer m_container = new CookieContainer();
         protected override WebRequest GetWebRequest(Uri address)
         {
             var request = base.GetWebRequest(address);
             if (request is HttpWebRequest)
             {
                 ((HttpWebRequest)request).CookieContainer = m_container;
             }
             return request;
         }
     }
}