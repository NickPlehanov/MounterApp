﻿using Microsoft.AppCenter.Analytics;
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
    public static class ClientHttp {
        //readonly BaseViewModel b = new BaseViewModel();
        public static HttpClientHandler GetHttpClientHandler() {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender,cert,chain,sslPolicyErrors) => { return true; };
            return clientHandler;
        }
        public static async Task<T> GetQuery<T>(string query) where T : class {
            Analytics.TrackEvent("Выполнение запроса",
            new Dictionary<string,string> {
                {"query",query },
                {"phone", Application.Current.Properties["Phone"].ToString() }
            });
            using HttpClient client = new HttpClient(GetHttpClientHandler());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            try {
                //HttpResponseMessage response = await client.GetAsync(new Uri(Resources.BaseAddress + query));
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + query).ConfigureAwait(false);
                if(response.IsSuccessStatusCode) {
                    var resp = response.Content.ReadAsStringAsync().Result;
                    if(string.IsNullOrEmpty(resp))
                        return null;
                    else {
                        try {
                            client.CancelPendingRequests();
                            client.Dispose();
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
                            client.CancelPendingRequests();
                            client.Dispose();
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
                    client.CancelPendingRequests();
                    client.Dispose();
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
                client.CancelPendingRequests();
                client.Dispose();
                return null;
            }
        }
        public static async Task<HttpStatusCode> PostQuery(string query,HttpContent content){
            Analytics.TrackEvent("Выполнение запроса",
            new Dictionary<string,string> {
                {"query",query },
                {"phone", Application.Current.Properties["Phone"].ToString() }
            });
            using HttpClient client = new HttpClient(GetHttpClientHandler());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            try {
                //HttpResponseMessage response = await client.PostAsync(new Uri(Resources.BaseAddress + query),content);
                HttpResponseMessage response = await client.PostAsync(Resources.BaseAddress + query,content).ConfigureAwait(false);
                if(response.IsSuccessStatusCode) {
                    //var resp = response.Content.ReadAsStringAsync().Result;
                    //if(string.IsNullOrEmpty(resp))
                    client.CancelPendingRequests();
                    client.Dispose();
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
                    client.CancelPendingRequests();
                    client.Dispose();
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
                client.CancelPendingRequests();
                client.Dispose();
                return HttpStatusCode.InternalServerError;
            }
        }
        public static async Task<HttpStatusCode> PutQuery(string query,HttpContent content) {
            Analytics.TrackEvent("Выполнение запроса",
            new Dictionary<string,string> {
                {"query",query },
                {"phone", Application.Current.Properties["Phone"].ToString() }
            });
            using HttpClient client = new HttpClient(GetHttpClientHandler());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            try {
                //HttpResponseMessage response = client.PutAsync(new Uri(Resources.BaseAddress + query),content).GetAwaiter().GetResult();
                HttpResponseMessage response = client.PutAsync(Resources.BaseAddress + query,content).GetAwaiter().GetResult();
                if(response.IsSuccessStatusCode) {
                    //var resp = response.Content.ReadAsStringAsync().Result;
                    //if(string.IsNullOrEmpty(resp))
                    client.CancelPendingRequests();
                    client.Dispose();
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
                    client.CancelPendingRequests();
                    client.Dispose();
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
                client.CancelPendingRequests();
                client.Dispose();
                return HttpStatusCode.InternalServerError;
            }
        }
        public static async Task<HttpStatusCode> GetQuery(string query) {
            Analytics.TrackEvent("Выполнение запроса",
            new Dictionary<string,string> {
                {"query",query },
                {"phone", Application.Current.Properties["Phone"].ToString() }
            });
            using HttpClient client = new HttpClient(GetHttpClientHandler());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.ExpectContinue = false;
            try {
                //HttpResponseMessage response = await client.GetAsync(new Uri(Resources.BaseAddress + query));
                HttpResponseMessage response = await client.GetAsync(Resources.BaseAddress + query).ConfigureAwait(false);
                Analytics.TrackEvent("Выполнение запроса",
                new Dictionary<string,string> {
                        {"query",query },
                        {"phone", Application.Current.Properties["Phone"].ToString() },
                        {"statusCode", response.StatusCode.ToString() }
                });
                client.CancelPendingRequests();
                client.Dispose();
                return response.StatusCode;
            }
            catch(Exception ex) {
                Crashes.TrackError(new Exception("Ошибка выполнения запроса"),
                new Dictionary<string,string> {
                        {"query",query },
                        {"phone", Application.Current.Properties["Phone"].ToString() },
                        {"Error", ex.Message }
                });
                client.CancelPendingRequests();
                client.Dispose();
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
