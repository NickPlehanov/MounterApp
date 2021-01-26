using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MounterApp.Properties;
using MounterApp.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MounterApp.Helpers {
    public class ClientHttp {
        BaseViewModel b = new BaseViewModel();
        //public async Task<string> GetQuery(string query) {
        //    using(HttpClient client = new HttpClient(b.GetHttpClientHandler())) {
        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.ConnectionClose = true;
        //        client.DefaultRequestHeaders.ExpectContinue = false;
        //        HttpResponseMessage response = await client.GetAsync(new Uri(Resources.BaseAddress + query));
        //        if(response.IsSuccessStatusCode) {
        //            var resp = response.Content.ReadAsStringAsync().Result;
        //            if(string.IsNullOrEmpty(resp))
        //                return null;
        //            else
        //                return resp;
        //        }
        //        else
        //            return null;
        //    }
        //}
        public async Task<T> GetQuery<T>(string query) where T : class {
            Analytics.TrackEvent("Выполнение запроса",
            new Dictionary<string,string> {
                {"query",query },
                {"phone", Application.Current.Properties["Phone"].ToString() }
            });
            using(HttpClient client = new HttpClient(b.GetHttpClientHandler())) {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.ConnectionClose = true;
                client.DefaultRequestHeaders.ExpectContinue = false;
                try {
                    HttpResponseMessage response = await client.GetAsync(new Uri(Resources.BaseAddress + query));
                    if(response.IsSuccessStatusCode) {
                        var resp = response.Content.ReadAsStringAsync().Result;
                        if(string.IsNullOrEmpty(resp))
                            return null;
                        else {
                            try {
                                return JsonConvert.DeserializeObject<T>(resp);
                            }
                            catch(Exception ex) {
                                Crashes.TrackError(new Exception("Ошибка десериализации результата запроса"),
                                new Dictionary<string,string> {
                                    {"query",query },
                                    {"phone", Application.Current.Properties["Phone"].ToString() },
                                    {"Error", ex.Message },
                                    {"response", resp }
                                });
                                return null;
                            }
                        }
                    }
                    else {
                        Analytics.TrackEvent("От сервера не получен корректный ответ",
                        new Dictionary<string,string> {
                            {"query",query },
                            {"responseStatusCode",response.StatusCode.ToString() },
                            {"response",response.ToString() },
                            {"phone", Application.Current.Properties["Phone"].ToString() }
                        });
                        return null;
                    }
                }
                catch(Exception ex) {
                    Crashes.TrackError(new Exception("Ошибка выполнения запроса"),
                    new Dictionary<string,string> {
                        {"query",query },
                        {"phone", Application.Current.Properties["Phone"].ToString() },
                        {"Error", ex.Message }
                    });
                    return null;
                }
            }
        }
        public async Task<HttpStatusCode> PostQuery(string query,HttpContent content){
            Analytics.TrackEvent("Выполнение запроса",
            new Dictionary<string,string> {
                {"query",query },
                {"phone", Application.Current.Properties["Phone"].ToString() }
            });
            using(HttpClient client = new HttpClient(b.GetHttpClientHandler())) {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.ConnectionClose = true;
                client.DefaultRequestHeaders.ExpectContinue = false;
                try {
                    HttpResponseMessage response = await client.PostAsync(new Uri(Resources.BaseAddress + query),content);
                    if(response.IsSuccessStatusCode) {
                        //var resp = response.Content.ReadAsStringAsync().Result;
                        //if(string.IsNullOrEmpty(resp))
                        return response.StatusCode;
                        //else {
                        //    try {
                        //        //return JsonConvert.DeserializeObject<T>(resp);
                        //    }
                        //    catch(Exception ex) {
                        //        Crashes.TrackError(new Exception("Ошибка десериализации результата запроса"),
                        //        new Dictionary<string,string> {
                        //            {"query",query },
                        //            {"phone", Application.Current.Properties["Phone"].ToString() },
                        //            {"Error", ex.Message },
                        //            {"response", resp }
                        //        });
                        //        return response.StatusCode;
                        //    }
                        //}
                    }
                    else {
                        Analytics.TrackEvent("От сервера не получен корректный ответ",
                        new Dictionary<string,string> {
                            {"query",query },
                            {"responseStatusCode",response.StatusCode.ToString() },
                            {"response",response.ToString() },
                            {"phone", Application.Current.Properties["Phone"].ToString() }
                        });
                        return response.StatusCode;
                    }
                }
                catch(Exception ex) {
                    Crashes.TrackError(new Exception("Ошибка выполнения запроса"),
                    new Dictionary<string,string> {
                        {"query",query },
                        {"phone", Application.Current.Properties["Phone"].ToString() },
                        {"Error", ex.Message }
                    });
                    return HttpStatusCode.InternalServerError;
                }
            }
        }
        public async Task<HttpStatusCode> PutQuery(string query,HttpContent content) {
            Analytics.TrackEvent("Выполнение запроса",
            new Dictionary<string,string> {
                {"query",query },
                {"phone", Application.Current.Properties["Phone"].ToString() }
            });
            using(HttpClient client = new HttpClient(b.GetHttpClientHandler())) {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.ConnectionClose = true;
                client.DefaultRequestHeaders.ExpectContinue = false;
                try {
                    HttpResponseMessage response = client.PutAsync(new Uri(Resources.BaseAddress + query),content).GetAwaiter().GetResult() ;
                    if(response.IsSuccessStatusCode) {
                        //var resp = response.Content.ReadAsStringAsync().Result;
                        //if(string.IsNullOrEmpty(resp))
                        return response.StatusCode;
                        //else {
                        //    try {
                        //        //return JsonConvert.DeserializeObject<T>(resp);
                        //    }
                        //    catch(Exception ex) {
                        //        Crashes.TrackError(new Exception("Ошибка десериализации результата запроса"),
                        //        new Dictionary<string,string> {
                        //            {"query",query },
                        //            {"phone", Application.Current.Properties["Phone"].ToString() },
                        //            {"Error", ex.Message },
                        //            {"response", resp }
                        //        });
                        //        return response.StatusCode;
                        //    }
                        //}
                    }
                    else {
                        Analytics.TrackEvent("От сервера не получен корректный ответ",
                        new Dictionary<string,string> {
                            {"query",query },
                            {"responseStatusCode",response.StatusCode.ToString() },
                            {"response",response.ToString() },
                            {"phone", Application.Current.Properties["Phone"].ToString() }
                        });
                        return response.StatusCode;
                    }
                }
                catch(Exception ex) {
                    Crashes.TrackError(new Exception("Ошибка выполнения запроса"),
                    new Dictionary<string,string> {
                        {"query",query },
                        {"phone", Application.Current.Properties["Phone"].ToString() },
                        {"Error", ex.Message }
                    });
                    return HttpStatusCode.InternalServerError;
                }
            }
        }
        public async Task<HttpStatusCode> GetQuery(string query) {
            Analytics.TrackEvent("Выполнение запроса",
            new Dictionary<string,string> {
                {"query",query },
                {"phone", Application.Current.Properties["Phone"].ToString() }
            });
            using(HttpClient client = new HttpClient(b.GetHttpClientHandler())) {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.ConnectionClose = true;
                client.DefaultRequestHeaders.ExpectContinue = false;
                try {
                    HttpResponseMessage response = await client.GetAsync(new Uri(Resources.BaseAddress + query));
                    Analytics.TrackEvent("Выполнение запроса",
                    new Dictionary<string,string> {
                        {"query",query },
                        {"phone", Application.Current.Properties["Phone"].ToString() },
                        {"statusCode", response.StatusCode.ToString() }
                    });
                    return response.StatusCode;
                }
                catch(Exception ex) {
                    Crashes.TrackError(new Exception("Ошибка выполнения запроса"),
                    new Dictionary<string,string> {
                        {"query",query },
                        {"phone", Application.Current.Properties["Phone"].ToString() },
                        {"Error", ex.Message }
                    });
                    return HttpStatusCode.InternalServerError;
                }
            }
        }
    }
}
