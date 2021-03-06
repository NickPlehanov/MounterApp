﻿using MounterApp.Properties;
using Newtonsoft.Json;
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

        static readonly HttpClient client = new HttpClient(GetClientHandeler());
        //static readonly HttpClient client = new HttpClient();

        public static async Task<T> Get<T>(string query) where T : class {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            CancellationTokenSource cts = new CancellationTokenSource(7000);
            HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress1 + query, HttpCompletionOption.ResponseContentRead, cts.Token);
            //httpResponse.EnsureSuccessStatusCode();
            if (httpResponse.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(await httpResponse.Content.ReadAsStringAsync());
            else
                return null;

        }
        public static async Task<string> GetString(string query) {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            CancellationTokenSource cts = new CancellationTokenSource(7000);
            HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress1 + query, HttpCompletionOption.ResponseContentRead, cts.Token);
            //httpResponse.EnsureSuccessStatusCode();
            if (httpResponse.IsSuccessStatusCode)
                return await httpResponse.Content.ReadAsStringAsync();
            else
                return null;
        }
        public static async Task<string> GetPhoto(string query) {
            try {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.ConnectionClose = true;
                client.DefaultRequestHeaders.ExpectContinue = false;
                CancellationTokenSource cts = new CancellationTokenSource(20000);
                HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress1 + query, HttpCompletionOption.ResponseContentRead, cts.Token);
                //httpResponse.EnsureSuccessStatusCode(); 
                if (httpResponse.IsSuccessStatusCode)
                    return await httpResponse.Content.ReadAsStringAsync();
                else
                    return null;
            }
            catch {
                return null;
            }
        }
        public static async Task<HttpStatusCode> Get(string query) {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            CancellationTokenSource cts = new CancellationTokenSource(7000);
            HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress1 + query, HttpCompletionOption.ResponseContentRead, cts.Token);
            return httpResponse.StatusCode;
        }
        public static async Task<string> PostString(string query, HttpContent content) {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            HttpResponseMessage httpResponse = await client.PostAsync(Resources.BaseAddress1 + query, content);
            if (httpResponse.IsSuccessStatusCode)
                return await httpResponse.Content.ReadAsStringAsync();
            else
                return null;
        }
        public static async Task<T> Post<T>(string query, HttpContent content) where T : class {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            HttpResponseMessage httpResponse = await client.PostAsync(Resources.BaseAddress1 + query, content);
            if (httpResponse.IsSuccessStatusCode)
                //return await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(await httpResponse.Content.ReadAsStringAsync());
            else
                return null;
        }
        public static async Task<HttpStatusCode> PostStateCode(string query, HttpContent content) {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            HttpResponseMessage httpResponse = await client.PostAsync(Resources.BaseAddress1 + query, content);
            return httpResponse.StatusCode;
        }
        public static async Task<HttpStatusCode> Put(string query, HttpContent content) {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            HttpResponseMessage httpResponse = await client.PutAsync(Resources.BaseAddress1 + query, content);
            return httpResponse.StatusCode;
        }
    }
}
