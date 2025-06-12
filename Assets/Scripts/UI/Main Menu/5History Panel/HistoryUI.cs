using System.Collections;
using UnityEngine;
using TMPro;

public class HistoryUI : MonoBehaviour {
    public Transform historyContainer;
    public GameObject historyEntryPrefab;

    void OnEnable() {
        StartCoroutine(FetchAndDisplayHistory());
    }

    private IEnumerator FetchAndDisplayHistory() {
        ExerciseService exerciseService = new ExerciseService();
        // Destroy it first
        yield return exerciseService.FetchLast10Exercises(
            onSuccess: response =>
            {
                // Clear previous entries (except header if you have one)
                for (int i = historyContainer.childCount - 1; i >= 1; i--)
                    Destroy(historyContainer.GetChild(i).gameObject);

                foreach (var exercise in response.data)
                {
                    var entry = Instantiate(historyEntryPrefab, historyContainer);
                    entry.transform.Find("ExerciseName").GetComponent<TMP_Text>().text = exercise.exercise;
                    entry.transform.Find("Result").GetComponent<TMP_Text>().text = exercise.earnedPoints.ToString();
                    entry.transform.Find("Difficulty").GetComponent<TMP_Text>().text = "Difficulty: " + exercise.difficulty;
                    entry.transform.Find("Date").GetComponent<TMP_Text>().text = "Date: " + exercise.completedAt.ToString();
                }
            },
            onError: err =>
            {
                Debug.Log(err.message);
                Debug.LogError("Failed to fetch exercise history: " + err);
            }
        );
    }
}