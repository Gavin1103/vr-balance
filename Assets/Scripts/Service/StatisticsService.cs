using System.Collections;
using System.Collections.Generic;
using DTO.Response;
using Models;
using Utils;

public class StatisticsService
{
    public IEnumerator GetFireflyLeaderboard(
        System.Action<ApiResponse<List<LeaderboardExerciseResponse>>> onSuccess,
        System.Action<ApiErrorResponse<Void>> onError) {
        yield return ApiClient.Get<ApiResponse<List<LeaderboardExerciseResponse>>, ApiErrorResponse<Void>>(
            "/statistics/public/firefly-leaderboard",
            response => onSuccess?.Invoke(response),
            error => onError?.Invoke(error)
        );
    }
}
