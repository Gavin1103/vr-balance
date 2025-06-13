using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewBalanceTest", menuName = "Exercise/Exercises/Balance")]
public class BalanceTestExerciseSO : GenericExerciseSO {
    public override Exercise CreateExercise() {
        List<ExerciseMovement> movements = GetMovements();
        List<PositionChecker> positionCheckers = GetPositionCheckers();

        return new BalanceTestExercise(BackendEnum, Title, Description, Requirements, movements, AmountOfSets, WaitTimeBetweenSets, AmountOfReps, WaitTimeBetweenReps, PositionNeeded, EasyDifficulty, MediumDifficulty, HardDifficulty, positionCheckers);
    }
}