using GUI.utils;
using LibraryGUI.Exceptions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace LibraryGUI.Lib
{
    public class NetworkClient : IDisposable
    {
        private readonly string _baseURL;
        private readonly HttpClient _httpClient;

        public NetworkClient(string BaseURL)
        {
            this._baseURL = BaseURL;
            this._httpClient = new HttpClient();
        }

        public T Get<T>(string queryString, string? token = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _baseURL + queryString);
            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = _httpClient.Send(request);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                APIResponse result = System.Text.Json.JsonSerializer.Deserialize<APIResponse>(content);
                if (typeof(T) == typeof(APIResponse))
                {
                    return System.Text.Json.JsonSerializer.Deserialize<T>(content);
                }
                else
                {
                    if (result.Data != null)
                    {
                        return (T)result.Data;
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
            else
            {
                string error = response.Content.ReadAsStringAsync().Result;
                throw new NetworkCommunicationException($"Kommunikációs hiba: \r\nResponse code: {response.StatusCode} \r\n {error}");
            }
        }

        public async Task<T> GetAsync<T>(string queryString, string? token = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _baseURL + queryString);
            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                APIResponse result = System.Text.Json.JsonSerializer.Deserialize<APIResponse>(content);
                if (typeof(T) == typeof(APIResponse))
                {
                    return System.Text.Json.JsonSerializer.Deserialize<T>(content);
                }
                else
                {
                    if (result.Data != null)
                    {
                        return (T)result.Data;
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new NetworkCommunicationException($"Kommunikációs hiba: {error}");
            }
        }


        public async Task<APIResponse> PostAsync<T>(string queryString, T data, string? token = null, string? contentType = "application/json")
        {
            APIResponse response = new APIResponse();

            try
            {
                using StringContent jsonContent = new(
                System.Text.Json.JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json");

                using (HttpResponseMessage httpResponse = await _httpClient.PostAsync(
                    _baseURL + queryString,
                    jsonContent))
                {
                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Hiba a kérés végrehajtása során! ({response.Message}) ({response.StatusCode})");
                        return default(APIResponse);
                    }

                    var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                    APIResponse result = System.Text.Json.JsonSerializer.Deserialize<APIResponse>(jsonResponse);

                    return result;
                }
            } catch (Exception ex)
            {
                response.StatusCode = 400;
                response.Message = $"{ex.Message}";
            }

            return response;
        }

        public void Dispose() {}
    }
}
