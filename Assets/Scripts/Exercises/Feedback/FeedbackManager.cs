using UnityEngine;
using TMPro;
using UnityEngine.UI;
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


    [Header("Feedback map")]
    public List<FeedbackData> feedbackDataList;
    private Dictionary<FeedbackType, FeedbackData> feedbackMap;

    private bool particlesAllowed { get { return SettingsManager.Instance.CurrentSettings.vfxEnabled; } }

    void Awake() {
        Instance = this;

        feedbackMap = new Dictionary<FeedbackType, FeedbackData>();
        foreach (var data in feedbackDataList) {
            feedbackMap[data.type] = data;
        }
    }

    // Spawn any feedback
    public void SpawnFeedback(string text, Color color, Vector3 spawnPosition, string soundName = null, ParticleSystem particles = null) {
        SoundManager.Instance.PlaySFX(soundName);

        GameObject obj = Instantiate(FeedbackTextPrefab, spawnPosition, Quaternion.identity);
        FeedbackText feedbackText = obj.GetComponent<FeedbackText>();
        feedbackText.Setup(text, color);

        if (particles != null && particlesAllowed) {
            ParticleSystem ps = Instantiate(particles, spawnPosition, Quaternion.identity);
            var mainModule = ps.main;
            mainModule.startColor = color;
            ps.Play();
            ps.transform.SetParent(obj.transform, true);
        }
    }

    // Handle generic feedback
    public void CalculateAndDisplayGenericFeedbackText(float score, float maxScore, Vector3 spawnPosition) {
        FeedbackType type = GetGenericFeedbackType(score, maxScore);
        DisplayFeedback(type, spawnPosition);
    }
    public void DisplayMissFeedback(Vector3 spawnPosition) {
        DisplayFeedback(FeedbackType.Miss, spawnPosition);
    }
    public void DisplayFeedback(FeedbackType type, Vector3 spawnPosition) {
        FeedbackData data = feedbackMap[type];
        SpawnFeedback(data.text, data.color, spawnPosition, data.soundName, data.particlePrefabs);
    }
    public FeedbackType GetGenericFeedbackType(float score, float maxScore) {
        float ratio = score / maxScore;

        if (ratio >= 0.95f) return FeedbackType.Perfect;
        if (ratio >= 0.85f) return FeedbackType.Great;
        if (ratio >= 0.70f) return FeedbackType.Good;
        if (ratio >= 0.50f) return FeedbackType.Okay;
        return FeedbackType.Bad;
    }
}