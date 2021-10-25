using Microsoft.AppCenter.Crashes;
using MounterApp.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MounterApp.Helpers {
    public static class ClientHttp {
        public static HttpClientHandler GetClientHandeler() {
            HttpClientHandler clientHandler = new HttpClientHandler {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            return clientHandler;
        }

        //static readonly HttpClient client = new HttpClient(GetClientHandeler());
        //static readonly HttpClient client = new HttpClient();

        public static async Task<T> Get<T>(string query) where T : class {
            HttpClient client = new HttpClient(GetClientHandeler());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.Timeout = TimeSpan.FromMinutes(5);
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            HttpResponseMessage httpResponse = null;
            try {
                httpResponse = await client.GetAsync(Resources.BaseAddress1 + query/*, HttpCompletionOption.ResponseContentRead*/, cts.Token);
                if (httpResponse.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<T>(await httpResponse.Content.ReadAsStringAsync());
                else if (cts.IsCancellationRequested) 
                    return null;
                else
                    return null;
            }
            catch (OperationCanceledException oce) {
                Dictionary<string, string> parameters = new Dictionary<string, string> {
                                    { "Query",query },
                                    { "Exception message",oce.Message },
                                    { "Exception StackTrace",oce.InnerException.StackTrace },
                                    { "IsCancellationRequested",cts.IsCancellationRequested.ToString() },
                                    { "ReasonPhrase",httpResponse.ReasonPhrase },
                                    { "StatusCode",httpResponse.StatusCode.ToString() },
                                    { "Content",httpResponse.Content.ToString() }
                                };
                Crashes.TrackError(oce, parameters);
                return null;
            }

        }
        public static async Task<string> GetString(string query) {
            HttpClient client = new HttpClient(GetClientHandeler());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.Timeout = TimeSpan.FromMinutes(5);
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress1 + query/*, HttpCompletionOption.ResponseContentRead*/, cts.Token);
            //httpResponse.EnsureSuccessStatusCode();
            if (httpResponse.IsSuccessStatusCode)
                return await httpResponse.Content.ReadAsStringAsync();
            else if (cts.IsCancellationRequested)
                return null;
            else {
                Dictionary<string, string> parameters = new Dictionary<string, string> {
                                    { "Query",query },
                                    //{ "Exception message",oce.Message },
                                    //{ "Exception StackTrace",oce.InnerException.StackTrace },
                                    { "IsCancellationRequested",cts.IsCancellationRequested.ToString() },
                                    { "ReasonPhrase",httpResponse.ReasonPhrase },
                                    { "StatusCode",httpResponse.StatusCode.ToString() },
                                    { "Content",httpResponse.Content.ToString() }
                                };
                Crashes.TrackError(new Exception("Вернулся null в Task<string> GetString "), parameters);
                return null;
            }
        }
        public static async Task<string> GetPhoto(string query) {
            try {
                HttpClient client = new HttpClient(GetClientHandeler());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.ConnectionClose = true;
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.Timeout = TimeSpan.FromMinutes(5);
                CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
                HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress1 + query/*, HttpCompletionOption.ResponseContentRead*/, cts.Token);
                //httpResponse.EnsureSuccessStatusCode(); 
                if (httpResponse.IsSuccessStatusCode)
                    return await httpResponse.Content.ReadAsStringAsync();
                else if (cts.IsCancellationRequested)
                    return null;
                else
                    return null;
            }
            catch {
                return null;
            }
        }
        public static async Task<HttpStatusCode> Get(string query) {
            HttpClient client = new HttpClient(GetClientHandeler());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.Timeout = TimeSpan.FromMinutes(5);
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress1 + query/*, HttpCompletionOption.ResponseContentRead*/, cts.Token);
            return httpResponse.StatusCode;
        }
        public static async Task<string> PostString(string query, HttpContent content) {
            HttpClient client = new HttpClient(GetClientHandeler());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.Timeout = TimeSpan.FromMinutes(5);
            HttpResponseMessage httpResponse = await client.PostAsync(Resources.BaseAddress1 + query, content);
            if (httpResponse.IsSuccessStatusCode)
                return await httpResponse.Content.ReadAsStringAsync();
            else
                return null;
        }
        public static async Task<T> Post<T>(string query, HttpContent content) where T : class {
            HttpClient client = new HttpClient(GetClientHandeler());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.Timeout = TimeSpan.FromMinutes(5);
            HttpResponseMessage httpResponse = await client.PostAsync(Resources.BaseAddress1 + query, content);
            if (httpResponse.IsSuccessStatusCode)
                //return await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(await httpResponse.Content.ReadAsStringAsync());
            else
                return null;
        }
        public static async Task<HttpStatusCode> PostStateCode(string query, HttpContent content) {
            HttpClient client = new HttpClient(GetClientHandeler());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.Timeout = TimeSpan.FromMinutes(5);
            HttpResponseMessage httpResponse = await client.PostAsync(Resources.BaseAddress1 + query, content);
            return httpResponse.StatusCode;
        }
        public static async Task<HttpStatusCode> Put(string query, HttpContent content) {
            HttpClient client = new HttpClient(GetClientHandeler());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.Timeout = TimeSpan.FromMinutes(5);
            HttpResponseMessage httpResponse = await client.PutAsync(Resources.BaseAddress1 + query, content);
            return httpResponse.StatusCode;
        }
    }
}
