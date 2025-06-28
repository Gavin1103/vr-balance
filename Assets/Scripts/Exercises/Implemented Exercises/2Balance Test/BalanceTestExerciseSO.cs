using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewBalanceTest", menuName = "Exercise/Balance")]
public class BalanceTestExerciseSO : GenericExerciseSO {
    public override Exercise CreateExercise() {
        List<ExerciseMovement> movements = GetMovements();
        
        BalanceTestExercise exercise = new BalanceTestExercise(BackendEnum, Category, Title, Description, Requirements);
        exercise.Movements = movements;

        RepsAndSetsSO RepsAndSetsConfig = ScriptableObject.CreateInstance<RepsAndSetsSO>();
        exercise.RepsAndSetsConfig = RepsAndSetsConfig.CreateConfig();
        return exercise;
    }
}