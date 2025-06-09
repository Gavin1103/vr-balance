using System.Collections;

public abstract class IMovementBehaviour {
    public ExerciseMovement ExerciseMovement;

    public abstract void OnMovementStart(ExerciseMovement movement);
    public abstract IEnumerator OnMovementUpdate(ExerciseMovement movement);
    public abstract void OnMovementEnd(ExerciseMovement movement);
}