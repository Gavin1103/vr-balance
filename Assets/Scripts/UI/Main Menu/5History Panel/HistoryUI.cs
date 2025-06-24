using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HistoryUI : MonoBehaviour {
    public Transform historyContainer;
    public GameObject historyEntryPrefab;
    public GameObject noHistoryFoundText;

    void OnEnable() {
        StartCoroutine(FetchAndDisplayHistory());
    }

    private IEnumerator FetchAndDisplayHistory() {
        // Ensure the history container is empty (except ze first one) before populating it
        for (int i = historyContainer.childCount - 1; i >= 1; i--)
            Destroy(historyContainer.GetChild(i).gameObject);

        noHistoryFoundText.SetActive(true);
        noHistoryFoundText.GetComponent<TMP_Text>().text = "Exercises don't get saved when playing as a guest.";

        ExerciseService exerciseService = new ExerciseService();
        yield return exerciseService.FetchLast10Exercises(
            onSuccess: response =>
            {
                // Clear previous entries (except header if you have one)
                for (int i = historyContainer.childCount - 1; i >= 1; i--)
                    Destroy(historyContainer.GetChild(i).gameObject);

                    if (response.data == null || response.data.Count == 0) {
                        noHistoryFoundText.SetActive(true);
                        noHistoryFoundText.GetComponent<TMP_Text>().text = "No exercise history found because you have never done anything. Play your first exercise!";
                        return;
                    } else {
                        noHistoryFoundText.SetActive(false);
                    }


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
                noHistoryFoundText.GetComponent<TMP_Text>().text = "Something went wrong trying to fetch an exercise. Error message: " + err.message;
                Debug.LogError("Failed to fetch exercise history: " + err);
            }
        );
    }
}