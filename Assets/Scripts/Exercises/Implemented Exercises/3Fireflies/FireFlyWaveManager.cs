//using Models.DTO.Exercise;
//using Models.User;
using Service;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FireFlyWaveManager : MonoBehaviour {
    public static FireFlyWaveManager FireFlyInstance { get; private set; }

    [SerializeField] private FirefliesSpawner firefliesSpawner;
    [SerializeField] private ButterFlySpawner butterfliesSpawner;
    [SerializeField] private int baseFireflyCount = -1; // Base number of fireflies per wave
    [SerializeField] private float timeBetweenWaves = 2f; // Delay before starting the next wave
    [SerializeField] private List<GameObject> nets;
    [SerializeField] private GameObject[] leaderboardRows;
    [SerializeField] private int wavesPerSession = 3; // Waves per session (can be easily adjusted)
    [SerializeField] private float breakDuration = 3f; // Break duration between sessions

    //private ExcerciseSerice excerciseSerice;
    private StatisticsService statistics;
    private int currentWave = 0;
    private int remainingFireflies;
    private int fireFliesCaught;
    private int completedSessions = 0; // Track the number of completed sessions

    protected virtual void Awake() {
        FireFlyInstance = this;
        //excerciseSerice = new ExcerciseSerice();
        statistics = new StatisticsService();
    }

    private void OnEnable() {
        FireFly.OnCaught += FireFly_OnCaught;
    }

    private void OnDisable() {
        FireFly.OnCaught -= FireFly_OnCaught;
    }

    private void FireFly_OnCaught() {
        remainingFireflies--;
        fireFliesCaught++;
        ScoreManager.Instance.AddScore(100);

        if (remainingFireflies <= 0) {
            Invoke(nameof(NextWave), timeBetweenWaves);
        }
    }

    public void StartWaves() {
        EnableNets();
        completedSessions = 0;
        StartSession();
    }

    private void StartSession() {
        if (completedSessions >= 2) {
            EndSession();
            return;
        }

        currentWave = 1;
        StartWave(currentWave);
    }

    private void StartWave(int waveNumber) {
        int spawnCount = baseFireflyCount + (waveNumber + 2);
        int fireflyType;
        remainingFireflies = spawnCount;

        switch (DifficultyManager.Instance.SelectedDifficulty) {
            case Difficulty.Easy:
                fireflyType = 0;
                firefliesSpawner.SpawnFireFly(spawnCount, fireflyType);
                break;
            case Difficulty.Medium:
                firefliesSpawner.SpawnRandomFireFly(spawnCount);
                break;
            case Difficulty.Hard:
                fireflyType = 0; // TEMP
                firefliesSpawner.SpawnRandomFireFly(spawnCount);
                butterfliesSpawner.SpawnButterfly(3,fireflyType);
                break;
            default:
                Debug.LogWarning("Not valid difficulty selected!");
                return;
        }
    }

    private void NextWave() {
        butterfliesSpawner.ClearAllButterflies();
        currentWave++;
        if (currentWave <= wavesPerSession) {
            StartWave(currentWave);
        } else {
            // After 3 waves, start the break period
            StartCoroutine(BreakSession());
        }
    }

    private IEnumerator BreakSession() {
        // After completing the waves in a session, give a break
        //DisableNets();
        Debug.Log("Break! Waiting for " + breakDuration + " seconds...");
        yield return new WaitForSeconds(breakDuration);

        completedSessions++;
        StartSession();
    }

    public async void EndSession() {
        // End the session and show the leaderboard
        Debug.Log("Session complete!");
        DisableNets();
        //SendFireflyData();
        //GetLeaderBoardData();
        ResetWave();
    }

    private void ResetWave() {
        currentWave = 0;
        remainingFireflies = 0;
        fireFliesCaught = 0;
        firefliesSpawner.ClearAllFireflies();
        butterfliesSpawner.ClearAllButterflies();
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

    //private void SendFireflyData() {
    //    CompletedFireflyExerciseDTO dto = new CompletedFireflyExerciseDTO {
    //        caughtFirefliesCount = fireFliesCaught,
    //        caughtWrongFirefliesCount = 100, // Adjust this value as per your needs
    //        earnedPoints = (int)ScoreManager.Instance.Score,
    //        difficulty = DifficultyManager.Instance.SelectedDifficulty,
    //        completedAt = System.DateTime.UtcNow
    //    };

    //    StartCoroutine(excerciseSerice.SaveEX(
    //        dto,
    //        onSuccess: ApiResponse => {
    //            Debug.Log(ApiResponse.message);
    //        },
    //        onError: error => {
    //            Debug.Log(error.message);
    //        }
    //    ));
    //}

    //private void GetLeaderBoardData() {
    //    StartCoroutine(statistics.GetLeaderboard(
    //        response => {
    //            Debug.Log("Success");

    //            var sorted = response.data
    //                .OrderByDescending(e => e.highscore)
    //                .Take(10)
    //                .ToList();

    //            for (int i = 0; i < leaderboardRows.Length; i++) {
    //                GameObject row = leaderboardRows[i];

    //                if (i < sorted.Count) {
    //                    var entry = sorted[i];

    //                    var nameText = row.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>();
    //                    var scoreText = row.transform.Find("Score").GetComponent<TMPro.TextMeshProUGUI>();

    //                    nameText.text = entry.username;
    //                    scoreText.text = entry.highscore.ToString();
    //                } else {
    //                    row.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = "---";
    //                    row.transform.Find("Score").GetComponent<TMPro.TextMeshProUGUI>().text = "";
    //                }
    //            }
    //        },
    //        error => {
    //            Debug.LogError("Failed to get leaderboard");
    //        }
    //    ));
    //}
}