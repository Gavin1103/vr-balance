using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceBehaviour : IMovementBehaviour {
    // This class is the same as the HoldBehaviour, but it is used for the Balance exercise to add HeadPositions for the headsway
    [HideInInspector] public List<Vector3> HeadPositions = new List<Vector3>();

    private float holdTime;
    private float elapsedWhileHolding = 0f;

    public BalanceBehaviour(float holdTime) {
        this.holdTime = holdTime;
    }

    public override void OnMovementStart(ExerciseMovement movement) {
        elapsedWhileHolding = 0f;
        GenericExerciseReferences.Instance.HoldMovementText.transform.parent.gameObject.SetActive(true);
        GenericExerciseReferences.Instance.HoldMovementText.text = holdTime.ToString("0.0") + "s";
        GenericExerciseReferences.Instance.HoldImageLine.sizeDelta = new Vector2(0, GenericExerciseReferences.Instance.HoldImageLine.sizeDelta.y);
    }

    public override IEnumerator OnMovementUpdate(ExerciseMovement movement) {
        while (elapsedWhileHolding < holdTime) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                break;
            }

            elapsedWhileHolding += Time.deltaTime;

            float elapsed = elapsedWhileHolding;
            GenericExerciseReferences.Instance.HoldImageLine.sizeDelta = new Vector2(
                Mathf.Lerp(0, 160, elapsed / holdTime),
                GenericExerciseReferences.Instance.HoldImageLine.sizeDelta.y);
            GenericExerciseReferences.Instance.HoldMovementText.text = (holdTime - elapsed).ToString("0.0") + "s";

            movement.exercise.ScoreCalculator.CalculateDistances(
                movement.LeftStickTarget, movement.RightStickTarget, movement.HeadTarget);
            movement.currentScore += movement.exercise.ScoreCalculator.CalculateDurationScore(movement.TotalScore, holdTime);

            HeadPositions.Add(ExerciseManager.Instance.Headset.transform.position);

            yield return null;
        }

        float avgSway = CalculateAverageSway(HeadPositions);
        DisplaySwayFeedback(avgSway);
    }
    
    private float CalculateAverageSway(List<Vector3> positions) {
        if (positions == null || positions.Count < 2) return 0f;

        float total = 0f;
        for (int i = 1; i < positions.Count; i++) {
            total += Vector3.Distance(positions[i], positions[i - 1]);
        }
        return total / (positions.Count - 1);
    }

    private void DisplaySwayFeedback(float sway) {
        Vector3 pos = GenericExerciseReferences.Instance.MovementImageObject.transform.position + new Vector3(0, 0.5f, 0);

        if (sway < 0.0003f) {
            FeedbackManager.Instance.SpawnFeedback("PERFECT!", Color.green, pos, "Action Image Perfect", BalanceTestExerciseReferences.Instance.perfectParticles);
        } else if (sway < 0.001f) {
            FeedbackManager.Instance.SpawnFeedback("GOOD", Color.yellow, pos, "Action Image Great", BalanceTestExerciseReferences.Instance.greatParticles);
        } else if (sway < 0.005f) {
            FeedbackManager.Instance.SpawnFeedback("OK", Color.gray, pos, "Action Image Good", BalanceTestExerciseReferences.Instance.goodParticles);
        } else {
            FeedbackManager.Instance.SpawnFeedback("TOO MUCH SWAY", Color.red, pos, "Action Image Miss", BalanceTestExerciseReferences.Instance.missParticles);
        }
    }

    public override void OnMovementEnd(ExerciseMovement movement) {
        GenericExerciseReferences.Instance.HoldMovementText.transform.parent.gameObject.SetActive(false);
        GenericExerciseReferences.Instance.HoldImageLine.sizeDelta = new Vector2(0, GenericExerciseReferences.Instance.HoldImageLine.sizeDelta.y);
    }
}