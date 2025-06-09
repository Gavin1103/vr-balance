using UnityEngine;

public abstract class ExerciseMovementSO : ScriptableObject {
    public float Duration;
    public Sprite Image;
    public float Score;

    public abstract ExerciseMovement CreateMovement();
}