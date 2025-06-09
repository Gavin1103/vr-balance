using UnityEngine;

[CreateAssetMenu(fileName = "NewMustMovement", menuName = "Exercise/Movement/Must")]
public class MustMovementeSO : AffordanceMovementSO {
    public override ExerciseMovement CreateMovement() {
        return new MustMovement(Left, Right, Head, Duration, Image, Score);
    }
}