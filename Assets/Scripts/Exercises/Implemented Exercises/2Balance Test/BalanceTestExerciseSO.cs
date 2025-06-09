using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewBalanceTest", menuName = "Exercise/Exercises/Balance")]
public class BalanceTestExerciseSO : NormalExerciseSO {
    public override Exercise CreateExercise() {
        List<ExerciseMovement> movements = new List<ExerciseMovement>();
        foreach (var moveSO in Movements) {
            ExerciseMovement movement = moveSO.CreateMovement();
            movements.Add(movement);
        }

        return new BalanceTestExercise(Title, Description, Requirements, movements, PositionNeeded, EasyDifficulty, MediumDifficulty, HardDifficulty);
    }
}