using Models.Authentication;
using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using Models.Environment;
using Models.User;
using Newtonsoft.Json;
using UnityEngine;
using Utils;
using Void = Models.Void;
using Models.DTO.Exercise;

public class ExcerciseSerice {
    public IEnumerator SaveEX(
            CompletedExerciseDTO request,
            Action<ApiResponse<Void>> onSuccess,
            Action<ApiErrorResponse<Void>> onError
        ) {
        string json = JsonConvert.SerializeObject(request);
        yield return ApiClient.Post<ApiResponse<Void>, ApiErrorResponse<Void>>(
            "/store-exercise/firefly",
            json,
            response => onSuccess?.Invoke(response),
            error => onError?.Invoke(error),
            PlayerPrefs.GetString("Login-Token")
            );
    }

}
