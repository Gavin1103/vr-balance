using System.Collections;
using UnityEngine;

public class MustBehaviour : IMovementBehaviour {
    public MustBehaviour() { 
        
    }
    
    public override void OnMovementStart(ExerciseMovement movement) {
        GenericExerciseReferences.Instance.InformationText.text = "Must Complete";
        GenericExerciseReferences.Instance.InformationObject.SetActive(true);
    }
    
    public override IEnumerator OnMovementUpdate(ExerciseMovement movement) {
        while (true) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                break;
            }

            movement.exercise.ScoreCalculator.CalculateDistances(movement.LeftStickTarget, movement.RightStickTarget, movement.HeadTarget);
            if (movement.exercise.ScoreCalculator.AllTargetsHit()) {
                if (movement.exercise.ScoreCalculator.IsAccurate()) {
                    break; // Continue only when target positions are accurately hit
                } else {
                    GenericExerciseReferences.Instance.InformationText.text = "Not Accurate Enough!";
                }
            } else {
                GenericExerciseReferences.Instance.InformationText.text = "Must Complete";
            }

            yield return null;
        }
    }
    
    public override void OnMovementEnd(ExerciseMovement movement) {
        GenericExerciseReferences.Instance.InformationObject.SetActive(false);
    }
}