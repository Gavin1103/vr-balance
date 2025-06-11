using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class EndScreenUI : MonoBehaviour {
    public static EndScreenUI Instance;

    public GameObject MainUI;
    public CanvasGroup EndScreenCanvasGroup;
    public TMP_Text TitleText;
    public TMP_Text ScoreText;
    private float durationBeforeEndUIShows = 0.5f;
    [SerializeField] private GameObject npc;

    void Awake() {
        Instance = this;
    }

    void OnEnable() {
        EndScreenCanvasGroup.alpha = 0;
        StartCoroutine(FadeIn());
    }

    public void UpdateEndUI(string exerciseName, string text) {
        string[] messages = {
            $"Great job completing the {exerciseName}!",
            $"You’ve finished the {exerciseName} – well done!",
            $"That’s it for the {exerciseName}. Nice work!",
            $"The {exerciseName} is complete – keep it up!",
            $"Well done! {exerciseName} finished successfully.",
            $"You just wrapped up the {exerciseName} – awesome!",
            $"That was the {exerciseName}. Great effort!",
            $"Nice! {exerciseName} completed and logged."
        };
        string selectedMessage = messages[Random.Range(0, messages.Length)];
        TitleText.text = selectedMessage;

        ScoreText.text = text;
    }

    private IEnumerator FadeIn() {
        yield return new WaitForSeconds(durationBeforeEndUIShows);

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration) {
            EndScreenCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        EndScreenCanvasGroup.alpha = 1;
    }

    public void ReturnToMainMenu() {
        MainUI.SetActive(true);
        this.gameObject.SetActive(false);
        npc.SetActive(false);
    }
}