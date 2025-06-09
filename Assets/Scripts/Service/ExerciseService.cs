using System;
using System.Collections;
using Models;
using Newtonsoft.Json;
using UnityEngine;
using Utils;
using Void = Models.Void;
using Models.DTO.Exercise;

public class ExerciseService
{
    public IEnumerator SaveEX(
        CompletedExerciseDTO request,
        Action<ApiResponse<Void>> onSuccess,
        Action<ApiErrorResponse<Void>> onError,
        string ApiEndpoint
    )
    {
        string userToken = PlayerPrefs.GetString("Login-Token");

        if (userToken != "")
        {
            string json = JsonConvert.SerializeObject(request);
            
            yield return ApiClient.Post<ApiResponse<Void>, ApiErrorResponse<Void>>(
                $"/store-exercise/{ApiEndpoint}",
                json,
                response => onSuccess?.Invoke(response),
                error => onError?.Invoke(error),
                userToken
            );
        }
    }
}