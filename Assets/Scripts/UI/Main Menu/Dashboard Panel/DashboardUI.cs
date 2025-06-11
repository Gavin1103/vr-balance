using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DTO.Response;
using DTO.Response.User;
using System.Collections.Generic;
using Service;
using TMPro;

public class DashboardUI : MonoBehaviour
{
    public TextMeshProUGUI streakText;
    public TextMeshProUGUI usernameText;
    private UserStatsService userStatsService = new UserStatsService();

    public Transform leaderboardContainer;
    public GameObject leaderboardEntryPrefab;
    public GameObject linePrefab;
    void OnEnable() {
        streakText.text = "Streak: N/A";
        usernameText.text = "Welcome Guest";
        HandleStreaks();
        HandleLeaderboard();
    }

    private void HandleStreaks() {
        StartCoroutine(userStatsService.GetUserStreak(
            // Login completed
            onSuccess: ApiResponse =>
            {
                OnStreakSuccess(ApiResponse);
            },
            // Error message
            onError: error =>
            {
                OnStreakError(error);
            }
        ));
    }

    private void OnStreakSuccess(ApiResponse<UserStreakDTO> response) {
        if (response.data != null) {
            int streak = response.data.currentStreak;
            streakText.text = $"Streak: {streak}";
            usernameText.text = $"Welcome {response.data.username}";
        } else {
            streakText.text = "Streak: 0";
        }
    }

    private void OnStreakError(ApiErrorResponse<Void> error)
    {
        streakText.text = "Streak: N/A";
    }
    
private void HandleLeaderboard()
{
    // Only remove children after the first two objects (Leaderboard text and line)
    for (int i = leaderboardContainer.childCount - 1; i >= 2; i--)
        Destroy(leaderboardContainer.GetChild(i).gameObject);

        // Get your best streak first
        StartCoroutine(userStatsService.GetUserStreak(
            onSuccess: userResponse =>
            {
                // "Your Best" entry
                var yourBestObj = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
                var nameText = yourBestObj.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>();
                var scoreText = yourBestObj.transform.Find("Score").GetComponent<TMPro.TextMeshProUGUI>();
                nameText.text = "Your Best";
                nameText.color = Color.cyan;
                scoreText.text = userResponse.data.highestStreak.ToString();
                scoreText.color = Color.cyan;

                // Line separator
                Instantiate(linePrefab, leaderboardContainer);

                // Now get top 20
                StartCoroutine(userStatsService.GetTop20HighestStreaks(
                    onSuccess: topResponse =>
                    {
                        for (int i = 0; i < topResponse.data.Count && i < 20; i++)
                        {
                            var entry = topResponse.data[i];
                            var entryObj = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
                            var entryNameText = entryObj.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>();
                            var entryScoreText = entryObj.transform.Find("Score").GetComponent<TMPro.TextMeshProUGUI>();
                            entryNameText.text = $"{i + 1}. {entry.username}";
                            entryScoreText.text = entry.highestStreak.ToString();

                            // Color coding
                            if (i == 0)
                            {
                                entryNameText.color = Color.red;
                                entryScoreText.color = Color.red;
                            }
                            else if (i == 1)
                            {
                                entryNameText.color = new Color(1f, 0.5f, 0f); // Orange
                                entryScoreText.color = new Color(1f, 0.5f, 0f);
                            }
                            else if (i == 2)
                            {
                                entryNameText.color = Color.yellow;
                                entryScoreText.color = Color.yellow;
                            }
                            else
                            {
                                entryNameText.color = Color.white;
                                entryScoreText.color = Color.white;
                            }
                        }
                    },
                    onError: err => { Debug.LogError("Failed to fetch top 20: " + err); }
                ));
            },
            onError: err => { Debug.LogError("Failed to fetch user streak: " + err); }
        ));
    }
}