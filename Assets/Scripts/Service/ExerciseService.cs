using System;
using System.Collections;
using System.Collections.Generic;
using DTO.Request.Exercise.@base;
using DTO.Response;
using Newtonsoft.Json;
using UnityEngine;
using Utils;
using Void = DTO.Response.Void;

public class ExerciseService
{
    public IEnumerator SaveExercise(
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
                $"/exercise/store-exercise/{ApiEndpoint}",
                json,
                response => onSuccess?.Invoke(response),
                error => onError?.Invoke(error),
                userToken
            );
        }
    }

    public IEnumerator FetchLast10Exercises(
        System.Action<ApiResponse<List<CompletedExerciseResponse>>> onSuccess,
        System.Action<ApiErrorResponse<Void>> onError)
    {
        string userToken = PlayerPrefs.GetString("Login-Token");

        if (userToken != "")
        {
            yield return ApiClient.Get<ApiResponse<List<CompletedExerciseResponse>>, ApiErrorResponse<Void>>(
                "/exercise/fetch-last-10-exercises",
                response => onSuccess?.Invoke(response),
                error => onError?.Invoke(error),
                userToken
            );
        }
    }
}