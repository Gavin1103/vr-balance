using UnityEngine;

[CreateAssetMenu(fileName = "NewCloseEyesMovement", menuName = "Exercise/Movement/Close Eyes")]
public class CloseEyesMovementSO : AffordanceMovementSO {
    public float closeEyesTime;
    public override ExerciseMovement CreateMovement() {
        return new CloseEyesMovement(Left, Right, Head, Duration, Image, Score, closeEyesTime);
    }
}