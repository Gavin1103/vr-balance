using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class FeedbackManager : MonoBehaviour {
    public static FeedbackManager Instance { get; private set; }

    [Header("UI Feedback")]
    public GameObject FeedbackTextPrefab;

    [System.Serializable]
    public class FeedbackData {
        public ParticleSystem particlePrefabs;
        public FeedbackType type;
        public string text;
        public Color color;
        public string soundName;
    }

    public enum FeedbackType {
        Perfect,
        Great,
        Good,
        Okay,
        Bad,
        Miss
    }

    [Header("Feedback map")]
    public List<FeedbackData> feedbackDataList;
    private Dictionary<FeedbackType, FeedbackData> feedbackMap;

    void Awake() {
        Instance = this;

        feedbackMap = new Dictionary<FeedbackType, FeedbackData>();
        foreach (var data in feedbackDataList) {
            feedbackMap[data.type] = data;
        }
    }

    public void CalculateAndDisplayFeedbackText(float score, float maxScore, Vector3 spawnPosition) {
        FeedbackType type = GetFeedbackType(score, maxScore);
        DisplayFeedback(type, spawnPosition);
    }

    public void DisplayMissFeedback(Vector3 spawnPosition) {
        DisplayFeedback(FeedbackType.Miss, spawnPosition);
    }

    public void DisplayFeedback(FeedbackType type, Vector3 spawnPosition) {
        FeedbackData data = feedbackMap[type];

        // Play sound
        SoundManager.soundInstance.PlaySFX(data.soundName);

        // Display text
        GameObject obj = Instantiate(FeedbackTextPrefab, spawnPosition, Quaternion.identity);
        FeedbackText feedbackText = obj.GetComponent<FeedbackText>();
        feedbackText.Setup(data.text, data.color);

        // Spawn particles
        if (data.particlePrefabs != null) {
            ParticleSystem ps = Instantiate(data.particlePrefabs, spawnPosition, Quaternion.identity);
            var mainModule = ps.main;
            mainModule.startColor = data.color;
            ps.Play();

            // Optional: parenten aan de feedbacktekst zodat het samen verdwijnt
            ps.transform.SetParent(obj.transform, true);
        }
    }

    public FeedbackType GetFeedbackType(float score, float maxScore) {
        float ratio = score / maxScore;

        if (ratio >= 0.95f) return FeedbackType.Perfect;
        if (ratio >= 0.85f) return FeedbackType.Great;
        if (ratio >= 0.70f) return FeedbackType.Good;
        if (ratio >= 0.50f) return FeedbackType.Okay;
        return FeedbackType.Bad;
    }
}