using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewNormalExercise", menuName = "Exercise/Exercises/Normal")]
public class NormalExerciseSO : ExerciseSO {
    public List<ExerciseMovementSO> Movements;

    public override Exercise CreateExercise() {
        List<ExerciseMovement> movements = new List<ExerciseMovement>();
        foreach (var moveSO in Movements) {
            ExerciseMovement movement = moveSO.CreateMovement();
            movements.Add(movement);
        }

        return new NormalExercise(Title, Description, Requirements, movements, PositionNeeded, EasyDifficulty, MediumDifficulty, HardDifficulty);
    }
}