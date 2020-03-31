using Evolent.Business.ServiceContracts;
using Evolent.Models.Shared;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Evolent.Business.Helpers
{
    public class ApiHelper
    {
        public string BaseUrl { get; set; }

        public ApiHelper(IMetaDataService metaDataService)
        {
            BaseUrl = metaDataService.BaseApiUrl;
        }

        /// <summary>
        /// HTTP GET API Helper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string apiPath, string queryStringParams, IEvolentUser evolentUser)
        {
            T apiResult = default(T);

            if (!string.IsNullOrEmpty(queryStringParams))
            {
                if (apiPath.Contains("?"))
                    apiPath = apiPath + "&" + queryStringParams;
                else if (queryStringParams.Contains("?"))
                    apiPath = apiPath + queryStringParams;
                else
                    apiPath = apiPath + "?" + queryStringParams;
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.AppToken))
                    client.DefaultRequestHeaders.Add("AppToken", evolentUser.AppToken);
                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.UserToken))
                    client.DefaultRequestHeaders.Add("UserToken", evolentUser.UserToken);
                if (evolentUser != null && evolentUser.UserId.HasValue)
                    client.DefaultRequestHeaders.Add("UserId", evolentUser.UserId.Value.ToString());

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.ClientIPAddress))
                    client.DefaultRequestHeaders.Add("ClientIPAddress", evolentUser.ClientIPAddress);

                HttpResponseMessage httpResponse = await client.GetAsync(apiPath);
                if (httpResponse.IsSuccessStatusCode)
                {
                    string result = httpResponse.Content.ReadAsStringAsync().Result;
                    apiResult = JsonConvert.DeserializeObject<T>(result);
                }
            }
            return apiResult;
        }

        /// <summary>
        /// HTTP GET API Helper
        /// Which returns string
        /// </summary>     
        /// <param name="apiPath"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string apiPath, string queryStringParams, IEvolentUser evolentUser)
        {
            string apiResult = string.Empty;

            if (!string.IsNullOrEmpty(queryStringParams))
            {
                if (apiPath.Contains("?"))
                    apiPath = apiPath + "&" + queryStringParams;
                else if (queryStringParams.Contains("?"))
                    apiPath = apiPath + queryStringParams;
                else
                    apiPath = apiPath + "?" + queryStringParams;
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.AppToken))
                    client.DefaultRequestHeaders.Add("AppToken", evolentUser.AppToken);
                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.UserToken))
                    client.DefaultRequestHeaders.Add("UserToken", evolentUser.UserToken);
                if (evolentUser != null && evolentUser.UserId.HasValue)
                    client.DefaultRequestHeaders.Add("UserId", evolentUser.UserId.Value.ToString());

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.ClientIPAddress))
                    client.DefaultRequestHeaders.Add("ClientIPAddress", evolentUser.ClientIPAddress);

                HttpResponseMessage httpResponse = await client.GetAsync(apiPath);
                if (httpResponse.IsSuccessStatusCode)
                {
                    apiResult = httpResponse.Content.ReadAsStringAsync().Result;
                }
            }
            return apiResult;
        }

        /// <summary>
        /// HTTP GET API Helper without user token requirement
        /// specifically created for the static data pull methods
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="evolentUser"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string apiPath, IEvolentUser evolentUser)
        {
            return await GetAsync<T>(apiPath, string.Empty, evolentUser);
        }

        /// <summary>
        /// HTTP GET API Helper without user token requirement
        /// specifically created for the static data pull methods
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="evolentUser"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string apiPath, IEvolentUser evolentUser)
        {
            return await GetAsync(apiPath, string.Empty, evolentUser);
        }

        /// <summary>
        /// HTTP POST API Helper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string apiPath, object requestData, IEvolentUser evolentUser)
        {
            T apiResult = default(T);
            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.AppToken))
                    client.DefaultRequestHeaders.Add("AppToken", evolentUser.AppToken);
                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.UserToken))
                    client.DefaultRequestHeaders.Add("UserToken", evolentUser.UserToken);
                if (evolentUser != null && evolentUser.UserId.HasValue)
                    client.DefaultRequestHeaders.Add("UserId", evolentUser.UserId.Value.ToString());

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.ClientIPAddress))
                    client.DefaultRequestHeaders.Add("ClientIPAddress", evolentUser.ClientIPAddress);

                HttpContent content = new StringContent(jsonStr);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage httpResponse = await client.PostAsync(apiPath, content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    string result = httpResponse.Content.ReadAsStringAsync().Result;
                    apiResult = JsonConvert.DeserializeObject<T>(result);
                }
                //TODO: What if IsSuccessStatusCode is FALSE
            }
            return apiResult;
        }

        /// <summary>
        /// HTTP POST API Helper
        /// </summary>       
        /// <param name="apiPath"></param>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public async Task<string> PostAsync(string apiPath, object requestData, IEvolentUser evolentUser)
        {
            string apiResult = string.Empty;
            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.AppToken))
                    client.DefaultRequestHeaders.Add("AppToken", evolentUser.AppToken);
                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.UserToken))
                    client.DefaultRequestHeaders.Add("UserToken", evolentUser.UserToken);
                if (evolentUser != null && evolentUser.UserId.HasValue)
                    client.DefaultRequestHeaders.Add("UserId", evolentUser.UserId.Value.ToString());

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.ClientIPAddress))
                    client.DefaultRequestHeaders.Add("ClientIPAddress", evolentUser.ClientIPAddress);

                HttpContent content = new StringContent(jsonStr);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage httpResponse = await client.PostAsync(apiPath, content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    apiResult = httpResponse.Content.ReadAsStringAsync().Result;
                }
                //TODO: What if IsSuccessStatusCode is FALSE
            }
            return apiResult;
        }

        /// <summary>
        /// HTTP DELETE API Helper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <returns></returns>
        public async Task<T> DeleteAsync<T>(string apiPath, string queryStringParams, IEvolentUser evolentUser)
        {
            T apiResult = default(T);

            if (queryStringParams != null)
            {
                if (apiPath.Contains("?"))
                    apiPath = apiPath + "&" + queryStringParams;
                else if (queryStringParams.Contains("?"))
                    apiPath = apiPath + queryStringParams;
                else
                    apiPath = apiPath + "?" + queryStringParams;
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.AppToken))
                    client.DefaultRequestHeaders.Add("AppToken", evolentUser.AppToken);
                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.UserToken))
                    client.DefaultRequestHeaders.Add("UserToken", evolentUser.UserToken);
                if (evolentUser != null && evolentUser.UserId.HasValue)
                    client.DefaultRequestHeaders.Add("UserId", evolentUser.UserId.Value.ToString());

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.ClientIPAddress))
                    client.DefaultRequestHeaders.Add("ClientIPAddress", evolentUser.ClientIPAddress);

                HttpResponseMessage httpResponse = await client.DeleteAsync(apiPath);
                if (httpResponse.IsSuccessStatusCode)
                {
                    string result = httpResponse.Content.ReadAsStringAsync().Result;
                    apiResult = JsonConvert.DeserializeObject<T>(result);
                }
            }
            return apiResult;
        }

        /// <summary>
        /// HTTP DELETE API Helper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <returns></returns>
        public async Task<string> DeleteAsync(string apiPath, string queryStringParams, IEvolentUser evolentUser)
        {
            string apiResult = string.Empty;

            if (queryStringParams != null)
            {
                if (apiPath.Contains("?"))
                    apiPath = apiPath + "&" + queryStringParams;
                else if (queryStringParams.Contains("?"))
                    apiPath = apiPath + queryStringParams;
                else
                    apiPath = apiPath + "?" + queryStringParams;
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.AppToken))
                    client.DefaultRequestHeaders.Add("AppToken", evolentUser.AppToken);
                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.UserToken))
                    client.DefaultRequestHeaders.Add("UserToken", evolentUser.UserToken);
                if (evolentUser != null && evolentUser.UserId.HasValue)
                    client.DefaultRequestHeaders.Add("UserId", evolentUser.UserId.Value.ToString());

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.ClientIPAddress))
                    client.DefaultRequestHeaders.Add("ClientIPAddress", evolentUser.ClientIPAddress);

                HttpResponseMessage httpResponse = await client.DeleteAsync(apiPath);
                if (httpResponse.IsSuccessStatusCode)
                {
                    apiResult = httpResponse.Content.ReadAsStringAsync().Result;
                }
            }
            return apiResult;
        }

        /// <summary>
        ///HTTP PUT API Helper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public async Task<T> PutAsync<T>(string apiPath, object RequestData, IEvolentUser evolentUser)
        {
            T apiResult = default(T);
            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(RequestData);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.AppToken))
                    client.DefaultRequestHeaders.Add("AppToken", evolentUser.AppToken);
                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.UserToken))
                    client.DefaultRequestHeaders.Add("UserToken", evolentUser.UserToken);
                if (evolentUser != null && evolentUser.UserId.HasValue)
                    client.DefaultRequestHeaders.Add("UserId", evolentUser.UserId.Value.ToString());

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.ClientIPAddress))
                    client.DefaultRequestHeaders.Add("ClientIPAddress", evolentUser.ClientIPAddress);

                HttpContent content = new StringContent(jsonStr);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage httpResponse = await client.PutAsync(apiPath, content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    string result = httpResponse.Content.ReadAsStringAsync().Result;
                    apiResult = JsonConvert.DeserializeObject<T>(result);
                }
            }
            return apiResult;
        }

        /// <summary>
        ///HTTP PUT API Helper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public async Task<string> PutAsync(string apiPath, object RequestData, IEvolentUser evolentUser)
        {
            string apiResult = string.Empty;
            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(RequestData);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.AppToken))
                    client.DefaultRequestHeaders.Add("AppToken", evolentUser.AppToken);
                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.UserToken))
                    client.DefaultRequestHeaders.Add("UserToken", evolentUser.UserToken);
                if (evolentUser != null && evolentUser.UserId.HasValue)
                    client.DefaultRequestHeaders.Add("UserId", evolentUser.UserId.Value.ToString());

                if (evolentUser != null && !string.IsNullOrEmpty(evolentUser.ClientIPAddress))
                    client.DefaultRequestHeaders.Add("ClientIPAddress", evolentUser.ClientIPAddress);

                HttpContent content = new StringContent(jsonStr);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage httpResponse = await client.PutAsync(apiPath, content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    apiResult = httpResponse.Content.ReadAsStringAsync().Result;
                }
            }
            return apiResult;
        }

    }
}
