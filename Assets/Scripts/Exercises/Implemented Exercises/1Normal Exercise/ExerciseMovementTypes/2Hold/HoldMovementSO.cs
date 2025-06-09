using UnityEngine;

[CreateAssetMenu(fileName = "NewHoldMovement", menuName = "Exercise/Movement/Hold")]
public class HoldMovementSO : AffordanceMovementSO {
    public float HoldTime;

    public override ExerciseMovement CreateMovement() {
        return new HoldMovement(Left, Right, Head, Duration, Image, Score, HoldTime);
    }
}