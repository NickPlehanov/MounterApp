using MounterApp.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MounterApp.Helpers {
    public static class ClientHttp {
        public static HttpClientHandler GetClientHandeler() {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender,cert,chain,sslPolicyErrors) => { return true; };
            return clientHandler;
        }

        static readonly HttpClient client = new HttpClient(GetClientHandeler());

        public static async Task<T> Get<T>(string query) where T:class{
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress + query);
            if(httpResponse.IsSuccessStatusCode) {
                return JsonConvert.DeserializeObject<T>(await httpResponse.Content.ReadAsStringAsync());
            }
            else
                return null;
        }
        public static async Task<string> GetString(string query){
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress + query);
            if (httpResponse.IsSuccessStatusCode) {
                return await httpResponse.Content.ReadAsStringAsync();
            }
            else
                return null;
        }
        public static async Task<string> GetPhoto(string query) {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress + query);
            if(httpResponse.IsSuccessStatusCode) {
                return await httpResponse.Content.ReadAsStringAsync();
            }
            else
                return null;
        }
        public static async Task<HttpStatusCode> Get(string query) {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            HttpResponseMessage httpResponse = await client.GetAsync(Resources.BaseAddress + query);
            return httpResponse.StatusCode;
        }
        public static async Task<string> Post(string query,HttpContent content) {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            HttpResponseMessage httpResponse = await client.PostAsync(Resources.BaseAddress + query,content);
            if(httpResponse.IsSuccessStatusCode) 
                return await httpResponse.Content.ReadAsStringAsync();            
            else
                return null;
        }
        public static async Task<HttpStatusCode> Put(string query,HttpContent content) {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            HttpResponseMessage httpResponse = await client.PutAsync(Resources.BaseAddress + query,content);
            return httpResponse.StatusCode;
            //if(httpResponse.IsSuccessStatusCode)
            //    return await httpResponse.Content.ReadAsStringAsync();
            //else
            //    return null;
        }
    }
}
