using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;


namespace Utils
{
    
    /// <summary>
    /// Provides utility methods for making HTTP requests (GET, POST, etc.) and handling JSON responses.
    /// </summary>
    /// <remarks>
    /// This static class supports generic deserialization and optional JWT authorization headers. 
    /// Use it to communicate with a backend API from Unity.
    /// </remarks>
    public static class ApiClient
    {
        /// <summary>
        /// The backend base url
        /// </summary>
        private static readonly string BaseUrl = "http://localhost:8080/api";

        /// <summary>
        /// Sends an HTTPS(dev-HTTP) POST request to the specified endpoint and processes the response as JSON.
        /// </summary>
        /// <typeparam name="TResponse">The expected response type after JSON deserialization.</typeparam>
        /// <param name="endpoint">The relative path of the API endpoint (e.g., "/auth/login").</param>
        /// <param name="bodyJson">The JSON string representing the request body.</param>
        /// <param name="onSuccess">Callback invoked with the deserialized response when the request succeeds.</param>
        /// <param name="onError">Callback invoked with an error message when the request fails.</param>
        /// <param name="jwt">Optional JWT token to be included in the Authorization header.</param>
        /// <returns>A coroutine that performs the network request.</returns>
        public static IEnumerator Post<TSuccess, TError>(
            string endpoint,
            string bodyJson,
            Action<TSuccess> onSuccess,
            Action<TError> onError,
            string jwt = null
        )
        {
            string fullUrl = BaseUrl + endpoint;
            UnityWebRequest request = UnityWebRequest.PostWwwForm(fullUrl, "");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJson ?? "");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            AddAuthHeader(request, jwt);

            yield return HandleRequest<TSuccess, TError>(request, onSuccess, onError);
        }

        /// <summary>
        /// Sends an HTTPS(dev-HTTP) GET request to the specified endpoint and processes the response as JSON.
        /// </summary>
        /// <typeparam name="TResponse">The expected response type after JSON deserialization.</typeparam>
        /// <param name="endpoint">The relative path of the API endpoint (e.g., "/user/me").</param>
        /// <param name="onSuccess">Callback invoked with the deserialized response when the request succeeds.</param>
        /// <param name="onError">Callback invoked with an error message when the request fails.</param>
        /// <param name="jwt">Optional JWT token to be included in the Authorization header.</param>
        /// <returns>A coroutine that performs the network request.</returns>
        public static IEnumerator Get<TSuccess, TError>(
            string endpoint,
            Action<TSuccess> onSuccess,
            Action<TError> onError,
            string jwt = null
        )
        {
            string fullUrl = BaseUrl + endpoint;

            UnityWebRequest request = UnityWebRequest.Get(fullUrl);
            request.SetRequestHeader("Content-Type", "application/json");
            AddAuthHeader(request, jwt);

            yield return HandleRequest(request, onSuccess, onError);
        }

        /// <summary>
        /// Adds a Bearer Authorization header to the request if a JWT token is provided.
        /// </summary>
        /// <param name="request">The UnityWebRequest to which the Authorization header will be added.</param>
        /// <param name="jwt">The JWT token to include in the Authorization header. If null or empty, no header is added.</param>
        private static void AddAuthHeader(UnityWebRequest request, string jwt)
        {
            if (!string.IsNullOrEmpty(jwt))
            {
                request.SetRequestHeader("Authorization", "Bearer " + jwt);
            }
        }

        /// <summary>
        /// Executes the provided UnityWebRequest and processes the response as JSON.
        /// </summary>
        /// <typeparam name="TResponse">The expected response type after JSON deserialization.</typeparam>
        /// <param name="request">The UnityWebRequest to be sent and handled.</param>
        /// <param name="onSuccess">Callback invoked with the deserialized response when the request succeeds.</param>
        /// <param name="onError">Callback invoked with an error message when the request fails or JSON parsing fails.</param>
        /// <returns>A coroutine that performs the network request and invokes the appropriate callback.</returns>
        private static IEnumerator HandleRequest<TSuccess, TError>(
            UnityWebRequest request,
            Action<TSuccess> onSuccess,
            Action<TError> onError
        )
        {
            yield return request.SendWebRequest();

            string json = request.downloadHandler.text;

            if (request.result != UnityWebRequest.Result.Success || request.responseCode >= 400)
            {
                try
                {
                    TError errorResponse = JsonConvert.DeserializeObject<TError>(json);
                    onError?.Invoke(errorResponse);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error parsing error response: " + ex.Message);
                    onError?.Invoke(default);
                }
            }
            else
            {
                try
                {
                    TSuccess successResponse = JsonConvert.DeserializeObject<TSuccess>(json);
                    onSuccess?.Invoke(successResponse);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error parsing error response: " + ex.Message);
                    Debug.LogError("Raw error json: " + json);
                    onError?.Invoke(default); // default = null
                }
            }
        }

    }
}