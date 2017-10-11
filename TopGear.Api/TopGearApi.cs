﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TopGear.Api.Models;

namespace TopGear.Api
{
  
    public static class TopGearApi<T>
    {
        private static HttpClient client = new HttpClient
        {
            BaseAddress = new Uri(ConfigurationManager.AppSettings["baseUrl"])
        };

        private static string Token = ConfigurationManager.AppSettings["Token"];

        public static Response<T> Get(string relativePath)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(relativePath).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<Response<T>>().Result;
            }
            else return new Response<T> { Sucesso = false };
        }

        public static Response<T> Get(int id, string relativePath)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(relativePath + "/PorId/" + id.ToString()).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<Response<T>>().Result;
                return result;
            }
            else return new Response<T> { Sucesso = false };
        }

        public static Response<int> Post(T objeto, string relativePath)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.PostAsJsonAsync(relativePath + "/post", MakeRequest(objeto)).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<Response<int>>().Result;
                return result;
            }
            else return new Response<int> { Sucesso = false };
        }

        public static Response<T> Put(T objeto, int id, string relativePath)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.PutAsJsonAsync(relativePath + "/Put/" + id.ToString(),
                                                                    MakeRequest(objeto)).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<Response<T>>().Result;
                return result;
            }
            else return new Response<T> { Sucesso = false };
        }

        public static Response<T> Delete(int id, string relativePath)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpRequestMessage request = new HttpRequestMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(TopGearApi<int>.MakeRequest(id)), Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = new Uri(client.BaseAddress + relativePath + "/Delete")
            };

            HttpResponseMessage response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<Response<T>>().Result;
                return result;
            }
            else return new Response<T> { Sucesso = false };
        }

        private static Request<T> MakeRequest(T dados)
        {
            return new Request<T> { Dados = dados, Token = Token };
        }
    }

}
