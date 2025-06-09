using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class GenericExerciseScoreCalculator {
    private float maxDistance = 1f; // Tolerance

    private float leftDist;
    private float rightDist;
    private float headDist;

    public float LeftPercentage { get; private set; }
    public float RightPercentage { get; private set; }
    public float HeadPercentage { get; private set; }
    public GenericExerciseScoreCalculator() {
    }

    public void CalculateDistances(Vector3 leftTarget, Vector3 rightTarget, Vector3 headTarget)
    {
        // Pythagoras
        leftDist = Vector3.Distance(ExerciseManager.Instance.LeftStick.position, leftTarget);
        rightDist = Vector3.Distance(ExerciseManager.Instance.RightStick.position, rightTarget);
        headDist = Vector3.Distance(ExerciseManager.Instance.Headset.position, headTarget);

        // Convert distance to "closeness" percentage
        LeftPercentage = Mathf.Clamp01(1f - (leftDist / maxDistance)) * 100f;
        RightPercentage = Mathf.Clamp01(1f - (rightDist / maxDistance)) * 100f;
        HeadPercentage = Mathf.Clamp01(1f - (headDist / maxDistance)) * 100f;

        // Update UI with "TOO FAR!" if percentage is 0
        GenericExerciseReferences.Instance.LeftTextPercentage.text = LeftPercentage <= 0f ? "L TOO FAR!" : $"L {LeftPercentage:F0}%";
        GenericExerciseReferences.Instance.RightTextPercentage.text = RightPercentage <= 0f ? "R TOO FAR!" : $"R {RightPercentage:F0}%";
        GenericExerciseReferences.Instance.HeadTextPercentage.text = HeadPercentage <= 0f ? "H TOO FAR!" : $"H {HeadPercentage:F0}%";

        // Update Affordance Pulses
        GenericExerciseReferences.Instance.LeftPulseAffordance.SetPulseScale(HeadPercentage);
        GenericExerciseReferences.Instance.RightPulseAffordance.SetPulseScale(HeadPercentage);
        GenericExerciseReferences.Instance.HeadsetPulseAffordance.SetPulseScale(HeadPercentage);
    }

    public bool IsAccurate() => LeftPercentage > 60 && RightPercentage > 60 && HeadPercentage > 60;
    public bool AllTargetsHit() => leftDist < maxDistance && rightDist < maxDistance && headDist < maxDistance;

    public float CalculateInstanceScore(float maxScore) {
        if (!AllTargetsHit()) return 0f;

        float score = ((LeftPercentage + RightPercentage + HeadPercentage) / 300f) * maxScore;
        ScoreManager.Instance.AddScore(score);
        return score;
    }

    public float CalculateDurationScore(float maxScore, float holdTime) {
        if (!AllTargetsHit()) return 0f;

        float framePercentage = (LeftPercentage + RightPercentage + HeadPercentage) / 3f;
        float frameScore = (framePercentage / 100f) * (maxScore / holdTime) * Time.deltaTime;
        frameScore = Mathf.Min(frameScore, maxScore);

        return frameScore;
    }

    public void DecideFeedback(float score, float maxScore) {
        Vector3 popupSpawnPosition = GenericExerciseReferences.Instance.MovementImageObject.transform.position + new Vector3(0, 0.5f, 0);

        // If any of the inputs are too far, you MISSED
        if (!AllTargetsHit()) {
            FeedbackManager.Instance.DisplayMissFeedback(popupSpawnPosition);
            return;
        }

        // Otherwise, normal feedback
        ScoreManager.Instance.AddScore(score);
        FeedbackManager.Instance.CalculateAndDisplayFeedbackText(score, maxScore, popupSpawnPosition);
    }
}