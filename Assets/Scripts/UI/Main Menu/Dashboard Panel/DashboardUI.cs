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

    public Button leaderboardAllTimeButton;
    public Button leaderboardCurrentButton;
    public Transform leaderboardAllTimeContainer;
    public Transform leaderboardCurrentContainer;
    public GameObject leaderboardEntryPrefab;
    public GameObject linePrefab;

    public void OnCurrentLeaderboardButtonPressed() {
        leaderboardAllTimeContainer.gameObject.SetActive(false);
        leaderboardCurrentContainer.gameObject.SetActive(true);
        UIStyler.ApplyStyle(leaderboardCurrentButton, true, true);
        UIStyler.ApplyStyle(leaderboardAllTimeButton, false, true);
    }
    public void OnAllTimeLeaderboardButtonPressed() {
        leaderboardCurrentContainer.gameObject.SetActive(false);
        leaderboardAllTimeContainer.gameObject.SetActive(true);
        UIStyler.ApplyStyle(leaderboardCurrentButton, false, true);
        UIStyler.ApplyStyle(leaderboardAllTimeButton, true, true);
    }
    void OnEnable()
    {
        streakText.text = "Streak: N/A";
        usernameText.text = "Welcome Guest";
        HandleStreaks();
        HandleLeaderboard();
    }

    private void HandleStreaks()
    {
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
    private void OnStreakSuccess(ApiResponse<UserStreakDTO> response)
    {
        if (response.data != null)
        {
            int streak = response.data.currentStreak;
            streakText.text = $"Streak: {streak}";
            usernameText.text = $"Welcome {response.data.username}";
        }
        else
        {
            streakText.text = "Streak: 0";
        }
    }
    private void OnStreakError(ApiErrorResponse<Void> error)
    {
        streakText.text = "Streak: N/A";
    }


    private void HandleLeaderboard()
{
    // All-time highest streaks
    StartCoroutine(userStatsService.GetTop20HighestStreaks(
        onSuccess: topResponse =>
        {
            PopulateLeaderboard(
                leaderboardAllTimeContainer,
                topResponse.data,
                entry => entry.username,
                entry => entry.highestStreak
            );
        },
        onError: err => { Debug.LogError("Failed to fetch all-time leaderboard: " + err); }
    ));

    // Current highest streaks
    StartCoroutine(userStatsService.GetTop20CurrentStreaks(
        onSuccess: topResponse =>
        {
            PopulateLeaderboard(
                leaderboardCurrentContainer,
                topResponse.data,
                entry => entry.username,
                entry => entry.currentStreak
            );
        },
        onError: err => { Debug.LogError("Failed to fetch current leaderboard: " + err); }
    ));
}
    

    private void PopulateLeaderboard<T>(Transform container, List<T> data, System.Func<T, string> getName, System.Func<T, int> getScore)
    {
        // Keep the first two children (title and line), remove the rest
        for (int i = container.childCount - 1; i >= 2; i--)
            Destroy(container.GetChild(i).gameObject);

        for (int i = 0; i < data.Count && i < 20; i++)
        {
            var entry = data[i];
            var entryObj = Instantiate(leaderboardEntryPrefab, container);
            var entryNameText = entryObj.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>();
            var entryScoreText = entryObj.transform.Find("Score").GetComponent<TMPro.TextMeshProUGUI>();
            entryNameText.text = $"{i + 1}. {getName(entry)}";
            entryScoreText.text = getScore(entry).ToString();

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
    }
}