# 1 | Feedback System

The feedback system shows how well the player performed by calculating a score ratio and matching it to a feedback type (like Perfect, Great, Okay, etc). When feedback is triggered, a sound is played, a colored popup appears, and optionally, particles are shown.

This system is useful for rewarding good performance and giving quick, readable feedback after every movement.

---

## 1.1 | `FeedbackData`

Each feedback type is defined in the Unity inspector using the `FeedbackData` class:

```csharp
[System.Serializable]
public class FeedbackData {
    public ParticleSystem particlePrefabs;
    public FeedbackType type;
    public string text;
    public Color color;
    public string soundName;
}
```

---

## 1.2 | Feedback Types

These are the different feedback categories used:

```csharp
public enum FeedbackType {
    Perfect,
    Great,
    Good,
    Okay,
    Bad,
    Miss
}
```

---

## 1.3 | How It Works

On startup, the system builds a dictionary that maps each feedback type to its data:

```csharp
void Awake() {
    Instance = this;

    feedbackMap = new Dictionary<FeedbackType, FeedbackData>();
    foreach (var data in feedbackDataList) {
        feedbackMap[data.type] = data;
    }

    particlesAllowed = true;
    particleToggle.onValueChanged.AddListener(OnToggleChanged);
}
```

When feedback needs to be shown, `CalculateAndDisplayFeedbackText()` determines which type should be used based on the score:

```csharp
public void CalculateAndDisplayFeedbackText(float score, float maxScore, Vector3 spawnPosition) {
    FeedbackType type = GetFeedbackType(score, maxScore);
    DisplayFeedback(type, spawnPosition);
}
```

---

## 1.4 | Displaying Feedback

The selected feedback type is displayed using:

```csharp
public void DisplayFeedback(FeedbackType type, Vector3 spawnPosition) {
    FeedbackData data = feedbackMap[type];

    // Play sound
    SoundManager.soundInstance.PlaySFX(data.soundName);

    // Display text
    GameObject obj = Instantiate(FeedbackTextPrefab, spawnPosition, Quaternion.identity);
    FeedbackText feedbackText = obj.GetComponent<FeedbackText>();
    feedbackText.Setup(data.text, data.color);

    // Spawn particles if allowed
    if (data.particlePrefabs != null && particlesAllowed) {
        ParticleSystem ps = Instantiate(data.particlePrefabs, spawnPosition, Quaternion.identity);
        var mainModule = ps.main;
        mainModule.startColor = data.color;
        ps.Play();

        // Optional: parent particles to text
        ps.transform.SetParent(obj.transform, true);
    }
}
```

To manually show a miss (e.g. when the user fails to do the action), use:

```csharp
DisplayMissFeedback(position);
```

---

## 1.5 | Toggle for Particles

The toggle in the UI allows turning particle effects on or off:

```csharp
private void OnToggleChanged(bool value) {
    particlesAllowed = value;
}
```

---

## 1.6 | Score Ratio Logic

The feedback type is chosen based on how close the score is to the maximum:

```csharp
public FeedbackType GetFeedbackType(float score, float maxScore) {
    float ratio = score / maxScore;

    if (ratio >= 0.95f) return FeedbackType.Perfect;
    if (ratio >= 0.85f) return FeedbackType.Great;
    if (ratio >= 0.70f) return FeedbackType.Good;
    if (ratio >= 0.50f) return FeedbackType.Okay;
    return FeedbackType.Bad;
}
```