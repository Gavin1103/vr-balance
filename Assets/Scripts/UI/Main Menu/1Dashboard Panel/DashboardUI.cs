using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DTO.Response;
using DTO.Response.User;
using Service;
using System.Collections.Generic;

public class DashboardUI : MonoBehaviour
{
    public TextMeshProUGUI streakText;
    public TextMeshProUGUI usernameText;

    // Leaderboard stuff
    public GameObject userStreakUI;
    public Button leaderboardAllTimeButton;
    public Transform leaderboardAllTime;
    public Transform leaderboardAllTimeContainer;
    public Button leaderboardCurrentButton;
    public Transform leaderboardCurrent;
    public Transform leaderboardCurrentContainer;
    public GameObject leaderboardEntryPrefab;
    public GameObject linePrefab;
    private UserStatsService userStatsService = new UserStatsService();
    private bool isGuest;

    // Advised exercises
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private GameObject advisedExerciseButtonPrefab;
    [SerializeField] private Transform advisedContainer;
    [SerializeField] private GameObject exercisePanel;
    [SerializeField] private Button exerciseTabButton;
    // Force firefly
    private void AddFireflyButton()
    {
        foreach (Transform child in advisedContainer)
        {
            Destroy(child.gameObject);
        }

        // forcing. the code here should def be replaced in ze future
        GameObject buttonObj = Instantiate(advisedExerciseButtonPrefab, advisedContainer);
        buttonObj.GetComponentInChildren<TMP_Text>().text = "Fireflies";
        Exercise exercise = ExerciseManager.Instance.FindExerciseByTitle("Fireflies");
        Image image = buttonObj.transform.Find("Image").GetComponent<Image>();
        image.sprite = exercise.Image;
        buttonObj.GetComponent<Button>().onClick.AddListener(OnFireflyExerciseButtonPressed);
    }
    public void OnFireflyExerciseButtonPressed() {
        mainMenuUI.SwitchToPanel(exercisePanel, exerciseTabButton);
        ExerciseManager.Instance.SelectExerciseByTitle("Fireflies");
    }

    void OnEnable()
    {
        AddFireflyButton();

        // Check if user is logged in or guest
        isGuest = !User.IsLoggedIn();
        userStreakUI.SetActive(!isGuest);
        streakText.text = "Streak: N/A";
        usernameText.text = isGuest ? "Welcome Guest" : "Welcome";

        LoadDashboard();
    }

    private void LoadDashboard()
    {
        if (isGuest)
        {
            LoadLeaderboards(null);
        }
        else
        {
            StartCoroutine(userStatsService.GetUserStreak(
                onSuccess: response =>
                {
                    usernameText.text = $"Welcome {response.data.username}";
                    streakText.text = $"Streak: {response.data.currentStreak}";
                    LoadLeaderboards(response.data);
                },
                onError: err =>
                {
                    Debug.LogWarning("User streak fetch failed.");
                    LoadLeaderboards(null);
                }
            ));
        }
    }

    private void LoadLeaderboards(UserStreakDTO userData)
    {
        StartCoroutine(userStatsService.GetTop20HighestStreaks(
            onSuccess: allTime =>
            {
                PopulateLeaderboard(
                    leaderboardAllTimeContainer,
                    userData?.highestStreak ?? -1,
                    userData != null ? "Your Highest" : null,
                    allTime.data,
                    e => e.username,
                    e => e.highestStreak
                );
            },
            onError: err => Debug.LogError("Failed to fetch all-time leaderboard.")
        ));

        StartCoroutine(userStatsService.GetTop20CurrentStreaks(
            onSuccess: current =>
            {
                PopulateLeaderboard(
                    leaderboardCurrentContainer,
                    userData?.currentStreak ?? -1,
                    userData != null ? "Your Current" : null,
                    current.data,
                    e => e.username,
                    e => e.currentStreak
                );
            },
            onError: err => Debug.LogError("Failed to fetch current leaderboard.")
        ));
    }

    private void PopulateLeaderboard<T>(
        Transform container,
        int userScore,
        string userLabel,
        List<T> data,
        System.Func<T, string> getName,
        System.Func<T, int> getScore
    )
    {
        // Clean up previous entries (keep title and line if any)
        for (int i = container.childCount - 1; i >= 2; i--)
            Destroy(container.GetChild(i).gameObject);

        if (!isGuest && userLabel != null)
        {
            InsertUserEntry(container, userLabel, userScore);
            Instantiate(linePrefab, container);
        }

        for (int i = 0; i < Mathf.Min(20, data.Count); i++)
        {
            var entry = data[i];
            var entryObj = Instantiate(leaderboardEntryPrefab, container);
            var nameText = entryObj.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            var scoreText = entryObj.transform.Find("Score").GetComponent<TextMeshProUGUI>();

            nameText.text = $"{i + 1}. {getName(entry)}";
            scoreText.text = getScore(entry).ToString();

            Color color = Color.white;
            if (i == 0) color = Color.red;
            else if (i == 1) color = new Color(1f, 0.5f, 0f); // Orange
            else if (i == 2) color = Color.yellow;

            nameText.color = color;
            scoreText.color = color;
        }
    }

    private void InsertUserEntry(Transform container, string label, int score)
    {
        var entryObj = Instantiate(leaderboardEntryPrefab, container);
        var nameText = entryObj.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        var scoreText = entryObj.transform.Find("Score").GetComponent<TextMeshProUGUI>();
        nameText.text = label;
        nameText.color = Color.cyan;
        scoreText.text = score.ToString();
        scoreText.color = Color.cyan;
    }

    public void OnCurrentLeaderboardButtonPressed()
    {
        leaderboardAllTime.gameObject.SetActive(false);
        leaderboardCurrent.gameObject.SetActive(true);
        UIStyler.ApplyStyle(leaderboardCurrentButton, true, true);
        UIStyler.ApplyStyle(leaderboardAllTimeButton, false, true);
    }

    public void OnAllTimeLeaderboardButtonPressed()
    {
        leaderboardCurrent.gameObject.SetActive(false);
        leaderboardAllTime.gameObject.SetActive(true);
        UIStyler.ApplyStyle(leaderboardCurrentButton, false, true);
        UIStyler.ApplyStyle(leaderboardAllTimeButton, true, true);
    }
}
