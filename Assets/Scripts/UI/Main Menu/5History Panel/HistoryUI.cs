using System.Collections;
using UnityEngine;
using TMPro;

public class HistoryUI : MonoBehaviour {
    public Transform historyContainer;
    public GameObject historyEntryPrefab;

    void Start() {
        StartCoroutine(FetchAndDisplayHistory());
    }

    private IEnumerator FetchAndDisplayHistory() {
        ExerciseService exerciseService = new ExerciseService();
        yield return exerciseService.FetchLast10Exercises(
            onSuccess: response => {
                // Clear previous entries (except header if you have one)
                for (int i = historyContainer.childCount - 1; i >= 1; i--)
                    Destroy(historyContainer.GetChild(i).gameObject);

                foreach (var exercise in response.data) {
                    var entry = Instantiate(historyEntryPrefab, historyContainer);
                    entry.transform.Find("ExerciseName").GetComponent<TMP_Text>().text = exercise.exerciseName;
                    entry.transform.Find("Date").GetComponent<TMP_Text>().text = exercise.date; // Adjust property as needed
                    // Add more fields as needed
                }
            },
            onError: err => {
                Debug.LogError("Failed to fetch exercise history: " + err);
            }
        );
    }
}