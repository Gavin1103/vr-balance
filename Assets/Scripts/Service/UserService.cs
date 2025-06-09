using System;
using System.Collections;
using DTO.Request.User;
using DTO.Response;
using DTO.Response.Authentication;
using Newtonsoft.Json;
using Utils;
using Void = DTO.Response.Void;

namespace Service
{
    public class UserService
    {
        public IEnumerator Login(
            UserLoginDTO request,
            Action<ApiResponse<JwtResponse>> onSuccess,
            Action<ApiErrorResponse<Void>> onError
        )
        {
            string json = JsonConvert.SerializeObject(request); 
            yield return ApiClient.Post<ApiResponse<JwtResponse>, ApiErrorResponse<Void>>(
                "/auth/login-pincode",
                json,
                response => onSuccess?.Invoke(response),
                error => onError?.Invoke(error)
            );
        }
    }
}