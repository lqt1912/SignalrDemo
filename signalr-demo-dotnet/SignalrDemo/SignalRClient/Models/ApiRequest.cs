using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SignalRClient.Models
{
    public class ApiRequest
    {
        public static HttpResponseMessage Post(string url, string param, object data, string mediaType)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue(mediaType));

            var result = client.PostAsync(param, new StringContent(ConvertToString(data), Encoding.UTF8, mediaType)).Result;
            client.Dispose();
            return result;
        }

        public static HttpResponseMessage Get(string url, string urlParameters, string mediaType)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(url);

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue(mediaType));

            var result = client.GetAsync(urlParameters).Result;
            client.Dispose();
            return result;
        }

        public static string ConvertToString(object data)
        {
            switch (data.GetType().Name)
            {
                case "String":
                    return data.ToString();

                default:
                    // Không dùng JsonConver. vì mấy class có JsonProperty sẽ parse ra theo nên gọi API không đúng param
                    var jss = new JavaScriptSerializer();
                    return jss.Serialize(data);
            }
        }

        public static T Format<T>(Object obj) => JsonConvert.DeserializeObject<T>(obj.ToString());
    }
}
