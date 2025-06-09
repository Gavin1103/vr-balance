using System.Collections;
using UnityEngine;

public class AffordanceBehaviour : IMovementBehaviour {

    public AffordanceBehaviour(){
        
    }

    public override void OnMovementStart(ExerciseMovement movement) {
        // Show balls
        GenericExerciseReferences.Instance.LeftStickAffordance.SetActive(true);
        GenericExerciseReferences.Instance.RightStickAffordance.SetActive(true);
        GenericExerciseReferences.Instance.HeadsetAffordance.SetActive(true);
        // Move affordances to target positions
        GenericExerciseReferences.Instance.LeftStickAffordance.transform.localPosition = ExerciseMovement.LeftStickTarget;
        GenericExerciseReferences.Instance.RightStickAffordance.transform.localPosition = ExerciseMovement.RightStickTarget;
        GenericExerciseReferences.Instance.HeadsetAffordance.transform.localPosition = ExerciseMovement.HeadTarget;
    }

    public override IEnumerator OnMovementUpdate(ExerciseMovement movement) {
        yield return null;
    }

    public override void OnMovementEnd(ExerciseMovement movement) {
        // Hide affordances
        GenericExerciseReferences.Instance.LeftStickAffordance.SetActive(false);
        GenericExerciseReferences.Instance.RightStickAffordance.SetActive(false);
        GenericExerciseReferences.Instance.HeadsetAffordance.SetActive(false);
    }
}