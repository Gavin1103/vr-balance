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
        movement.exercise.ScoreCalculator.DecideFeedback(movement.currentScore, movement.TotalScore);
    }

    public override void OnMovementEnd(ExerciseMovement movement) {
        GenericExerciseReferences.Instance.HoldMovementText.transform.parent.gameObject.SetActive(false);
        GenericExerciseReferences.Instance.HoldImageLine.sizeDelta = new Vector2(0, GenericExerciseReferences.Instance.HoldImageLine.sizeDelta.y);
    }
}