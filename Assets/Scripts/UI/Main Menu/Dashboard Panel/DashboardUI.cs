using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DTO.Response;
using DTO.Response.User;
using System.Collections.Generic;
using Service;
using TMPro;

public class DashboardUI : MonoBehaviour {
    public TextMeshProUGUI streakText;
    private UserStatsService userStatsService = new UserStatsService();
    void Start() {
        HandleUsername();
        HandleStreaks();
        HandleLeaderboard();
    }
    
    private void HandleUsername() {
        
    }
    
    private void HandleStreaks() {
        StartCoroutine(userStatsService.GetUserStreak(
            // Login completed
            onSuccess: ApiResponse => {
                OnStreakSuccess(ApiResponse);
            },
            // Error message
            onError: error => {
                OnStreakError(error);
            }
        ));
    }

    private void OnStreakSuccess(ApiResponse<UserStreakDTO> response) {
        if (response.data != null) {
            int streak = response.data.currentStreak;
            streakText.text = $"Streak: {streak}";
        } else {
            streakText.text = "Streak: 0";
        }
        Debug.Log("Streak fetched successfully: " + response.message);
    }

    private void OnStreakError(ApiErrorResponse<Void> error) {
        streakText.text = "Streak: N/A";
    }
    
    private void HandleLeaderboard() {
        
    }
}