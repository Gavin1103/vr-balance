using System.Collections;
using UnityEngine;

public class MustBehaviour : IMovementBehaviour {
    private float holdTime = 3f;
    private float elapsedWhileHolding = 0f;
    public MustBehaviour(float HoldTime) {
        holdTime = HoldTime;
    }
    
    public override void OnMovementStart(ExerciseMovement movement) {
        GenericExerciseReferences.Instance.InformationText.text = "Must Be Accurate To Proceed";
        GenericExerciseReferences.Instance.InformationObject.SetActive(true);
    }
    
    public override IEnumerator OnMovementUpdate(ExerciseMovement movement) {
        while (true) {
            if (Input.GetKeyDown(KeyCode.Space)) break;

            movement.exercise.ScoreCalculator.CalculateDistances(movement.LeftStickTarget, movement.RightStickTarget, movement.HeadTarget);

            if (movement.exercise.ScoreCalculator.AllTargetsHit() && movement.exercise.ScoreCalculator.IsAccurate()) {
                elapsedWhileHolding += Time.deltaTime;
                GenericExerciseReferences.Instance.InformationText.text = (holdTime - elapsedWhileHolding).ToString("0.0") + "s";
            } else {
                elapsedWhileHolding = 0f;
                GenericExerciseReferences.Instance.InformationText.text = "Not Accurate Enough!";
            }

            if (elapsedWhileHolding >= holdTime) break;

            yield return null;
        }
    }
    
    public override void OnMovementEnd(ExerciseMovement movement) {
        GenericExerciseReferences.Instance.InformationObject.SetActive(false);
    }
}