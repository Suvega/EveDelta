using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Xml;
using System.Threading.Tasks;
using System.IO;

namespace EveDelta.EveApi.http
{
    internal class httpServiceClient
    {
        private static HttpClientHandler _handler = new HttpClientHandler();





        /// <summary>
        /// Provides a async way to call a webservice endpoint
        /// </summary>
        /// <param name="url">URL for the service call</param>
        /// <param name="sensitiveParams">When a URL contains {}, sensitiveParams are inserted just before calling the service</param>
        /// <returns></returns>
        public static async Task<string> asyncHttp(string url, string sensitiveParams)
        {
            int loc = url.IndexOf("{}");
            string finalUrl = url;

            if (sensitiveParams != null)
            {
                if (loc > 0)
                {
                    finalUrl = url.Substring(0, loc) + sensitiveParams;
                    if (url.Length > loc + 2)
                        finalUrl += url.Substring(loc + 2);
                }
            }

            else
            {
                if (loc > 0)
                    throw new ArgumentException("URL contains {}, and therefore requires sensitive params being passed to asyncHttp", sensitiveParams);
            }

            HttpClient httpClient = new HttpClient(_handler);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, finalUrl);

            if (_handler.SupportsTransferEncodingChunked())
            {
                request.Headers.TransferEncodingChunked = true;
            }

            HttpResponseMessage response = await httpClient.SendAsync(request);
            string stResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return stResponse;
            }

            return null;
        }
    }
}
