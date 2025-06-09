using UnityEngine;

[CreateAssetMenu(fileName = "NewNormalMovement", menuName = "Exercise/Movement/Normal")]
public class NormalMovementSO : AffordanceMovementSO {
    public override ExerciseMovement CreateMovement() {
        return new NormalMovement(Left, Right, Head, Duration, Image, Score);
    }
}