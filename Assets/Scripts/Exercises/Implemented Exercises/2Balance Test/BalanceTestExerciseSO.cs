using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewBalanceTest", menuName = "Exercise/Exercises/Balance")]
public class BalanceTestExerciseSO : GenericExerciseSO {
    public override Exercise CreateExercise() {
        List<ExerciseMovement> movements = GetMovements();
        List<PositionChecker> positionCheckers = GetPositionCheckers();
        
        BalanceTestExercise exercise = new BalanceTestExercise(BackendEnum, Category, Title, Description, Requirements, PositionNeeded, EasyDifficulty, MediumDifficulty, HardDifficulty, positionCheckers);
        exercise.Movements = movements;

        RepsAndSetsSO RepsAndSetsConfig = ScriptableObject.CreateInstance<RepsAndSetsSO>();
        exercise.RepsAndSetsConfig = RepsAndSetsConfig.CreateConfig();
        return exercise;
    }
}