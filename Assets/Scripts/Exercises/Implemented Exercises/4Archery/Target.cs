using UnityEngine;
using TMPro;

public class Target : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI timerText;
    private float despawnTime;
    private float timeLeft;
    private bool isDespawning = false;
    private ArcheryExercise exercise;

    public void StartDespawnCountdown(float duration, ArcheryExercise exercise) { // Called from ArcheryExercise
        despawnTime = duration;
        timeLeft = despawnTime;
        isDespawning = true;

        this.exercise = exercise;
    }

    private void Update() {
        if (!isDespawning) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft < 0f) {
            FeedbackManager.Instance.DisplayMissFeedback(transform.position);
            DestroyTarget();
            return;
        }

        UpdateTimerText(timeLeft);
    }

    private void UpdateTimerText(float time) {
        timerText.text = time.ToString("F1");

        // Color lerp from white to red as time approaches zero
        float t = Mathf.InverseLerp(0f, despawnTime, time);
        timerText.color = Color.Lerp(Color.red, Color.white, t);
    }

    public void DestroyTarget() {
        exercise.activeTargets.Remove(gameObject);
        Destroy(gameObject);
    }
}