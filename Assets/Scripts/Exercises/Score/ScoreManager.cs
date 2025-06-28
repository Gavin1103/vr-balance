using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour {
    public static ScoreManager Instance { get; private set; }

    public float Score = 0;

    void Awake() {
        Instance = this;
    }

    public void AddScore(float amount) {
        Score += amount;
    }

    public void ResetScore() {
        Score = 0;
    }
}