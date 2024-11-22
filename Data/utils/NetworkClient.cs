using GUI.utils;
using MD_Networking.Exceptions;
using System.Net.Http.Headers;
using System.Text;

/*
 
 @Author: Daniel Molnar
 
 */
namespace MD_Networking
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
                throw new NetworkCommunicationException($"Network error: \r\nResponse code: {response.StatusCode} \r\n {error}");
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
                throw new NetworkCommunicationException($"Network error: {error}");
            }
        }

        public async Task<APIResponse> PostAsync<T>(string queryString, T data)
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
                        return default(APIResponse);
                    }

                    var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                    APIResponse result = System.Text.Json.JsonSerializer.Deserialize<APIResponse>(jsonResponse);

                    return result;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.Message = $"{ex.Message}";
            }

            return response;
        }

        public async Task<APIResponse> PostMultipartAsync(string queryString, MultipartFormDataContent data)
        {
            APIResponse response = new APIResponse();

            try
            {
                using (HttpResponseMessage httpResponse = await _httpClient.PostAsync(
                    _baseURL + queryString,
                    data))
                {
                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        throw new Exception(httpResponse.ReasonPhrase);
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

        public async Task<T> PostOrThrowAsync<T>(string queryString, T data)
        {
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
                        throw new NetworkCommunicationException($"Error! Invalid request {httpResponse.ReasonPhrase}");
                    }

                    var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                    return (T) System.Text.Json.JsonSerializer.Deserialize<T>(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                throw new NetworkCommunicationException($"Error! Invalid request {ex.Message}");
            }
        }

        public void Dispose() {}
    }
}
