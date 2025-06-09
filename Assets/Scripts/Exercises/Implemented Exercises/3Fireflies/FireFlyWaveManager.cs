using Models.DTO.Exercise;
using Models.User;
using Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FireFlyWaveManager : MonoBehaviour {

    public static FireFlyWaveManager FireFlyInstance { get; private set; }

    [SerializeField] private FirefliesSpawner spawner;
    [SerializeField] private int baseFireflyCount = 3; // Base number of fireflies per wave
    [SerializeField] private float timeBetweenWaves = 2f; // Delay before starting the next wave
    [SerializeField] private List<GameObject> nets;
    [SerializeField] private GameObject[] leaderboardRows;

    ExerciseService exerciseService;
    StatisticsService statistics;
    private int currentWave = 0;
    private int remainingFireflies;
    private int fireFliesCaught;
    protected virtual void Awake() {
        FireFlyInstance = this;
        exerciseService = new ExerciseService();
        statistics = new StatisticsService();
        
    }
    private void OnEnable() {
        // Subscribe to the static event when a Firefly is caught
        FireFly.OnCaught += FireFly_OnCaught;
    }
    private void OnDisable() {
        // Unsubscribe when disabled to prevent memory leaks or double calls
        FireFly.OnCaught -= FireFly_OnCaught;
    }

    private void FireFly_OnCaught() {
        // Decrease the counter each time a firefly is caught
        remainingFireflies--;
        fireFliesCaught++;
        ScoreManager.Instance.AddScore(100);
        // If all fireflies are caught, start the next wave after a delay
        if (remainingFireflies <= 0) {
            Invoke(nameof(NextWave), timeBetweenWaves);
        }
    }
    public void StartWaves() {
        // Start the first wave
        EnableNets();

        currentWave = 1;
        StartWave(currentWave);

    }
    public async void StopWaves() {
        CancelInvoke();
        DisableNets();

        SendFireflyData();        
        GetLeaderBoardData2();

        ResetWave();
    }
    private void StartWave(int waveNumber) {
        // Determine how many fireflies to spawn this wave
        int spawnCount = baseFireflyCount + (waveNumber + 2);
        remainingFireflies = spawnCount;

        switch (DifficultyManager.Instance.SelectedDifficulty) {
            case Difficulty.Easy:
                break;

            case Difficulty.Medium:
                break;

            case Difficulty.Hard:
                break;

            default:
                Debug.Log(DifficultyManager.Instance.SelectedDifficulty);
                Debug.LogWarning("Not valid difficulty selected!");
                return;
        }
        // Tell the spawner to spawn the fireflies
        spawner.SpawnFireFly(spawnCount);
    }
    private void EnableNets() {
        foreach (GameObject net in nets) {
            net.SetActive(true);
        }
    }
    private void DisableNets() {
        foreach (GameObject net in nets) {
            net.SetActive(false);
        }
    }
    private void NextWave() {
        // Increase the wave count and start the next wave
        currentWave++;
        StartWave(currentWave);
    }

    private void ResetWave() {
        currentWave = 0;
        remainingFireflies = 0;
        fireFliesCaught = 0;
        spawner.ClearAllFireflies();
    }

    private void SendFireflyData() {
        CompletedFireflyExerciseDTO dto = new CompletedFireflyExerciseDTO();
        dto.caughtFirefliesCount = fireFliesCaught;
        dto.caughtWrongFirefliesCount = 100;
        dto.earnedPoints = (int)ScoreManager.Instance.Score;
        dto.difficulty = DifficultyManager.Instance.SelectedDifficulty;
        dto.completedAt = System.DateTime.UtcNow;
        
        StartCoroutine(exerciseService.SaveEX(
            dto,
                // Login completed
                onSuccess: ApiResponse => {
                    Debug.Log(ApiResponse.message);
                },
                // Error message
                onError: error => {
                    Debug.Log(error.message);
                }
                ,
                "firefly"
        ));

    }

    private void GetLeaderBoardData() {
        StartCoroutine(statistics.GetLeaderboard(
    response => {
        Debug.Log("succes");
        foreach (var entry in response.data) {
            Debug.Log($"User: {entry.username}, Points: {entry.highscore}");
        }
    },
    error => {
        Debug.LogError("Failed to get leaderboard");
    }));
    }

    private void GetLeaderBoardData2() {
        StartCoroutine(statistics.GetLeaderboard(
            response => {
                Debug.Log("Succes");

                var sorted = response.data
                    .OrderByDescending(e => e.highscore)
                    .Take(10)
                    .ToList();

                for (int i = 0; i < leaderboardRows.Length; i++) {
                    GameObject row = leaderboardRows[i];

                    if (i < sorted.Count) {
                        var entry = sorted[i];

                        // Zoek de naam en score tekstvelden
                        var nameText = row.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>();
                        var scoreText = row.transform.Find("Score").GetComponent<TMPro.TextMeshProUGUI>();

                        nameText.text = entry.username;
                        scoreText.text = entry.highscore.ToString();
                    }
                    else {
                        // Als er minder dan 10 scores zijn, leeg maken
                        row.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = "---";
                        row.transform.Find("Score").GetComponent<TMPro.TextMeshProUGUI>().text = "";
                    }
                }
            },
            error => {
                Debug.LogError("Failed to get leaderboard");
            }));
    }

}
