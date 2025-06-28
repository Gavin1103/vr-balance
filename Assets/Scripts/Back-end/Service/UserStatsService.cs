using System.Collections;
using System.Collections.Generic;
using DTO.Response;
using DTO.Response.Statistics;
using DTO.Response.User;
using UnityEngine;
using Utils;

namespace Service
{
    public class UserStatsService
    {
        public IEnumerator GetTop20HighestStreaks(
            System.Action<ApiResponse<List<HighestStreakRankingDTO>>> onSuccess,
            System.Action<ApiErrorResponse<Void>> onError)
        {
            yield return ApiClient.Get<ApiResponse<List<HighestStreakRankingDTO>>, ApiErrorResponse<Void>>(
                "/user-stats/public/top-20-highest-streaks",
                response => onSuccess?.Invoke(response),
                error => onError?.Invoke(error)
            );
        }

        public IEnumerator GetTop20CurrentStreaks(
            System.Action<ApiResponse<List<CurrentStreakRankingDTO>>> onSuccess,
            System.Action<ApiErrorResponse<Void>> onError)
        {
            yield return ApiClient.Get<ApiResponse<List<CurrentStreakRankingDTO>>, ApiErrorResponse<Void>>(
                "/user-stats/public/top-20-current-streaks",
                response => onSuccess?.Invoke(response),
                error => onError?.Invoke(error)
            );
        }

        public IEnumerator GetUserStreak(
            System.Action<ApiResponse<UserStreakDTO>> onSuccess,
            System.Action<ApiErrorResponse<Void>> onError)
        {

            if (User.IsLoggedIn())
            {
                yield return ApiClient.Get<ApiResponse<UserStreakDTO>, ApiErrorResponse<Void>>(
                    "/user-stats/get-streak",
                    response => onSuccess?.Invoke(response),
                    error => onError?.Invoke(error),
                    User.GetToken()
                );
            }
        }
    }
}