using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewBalanceTest", menuName = "Exercise/Balance")]
public class BalanceTestExerciseSO : GenericExerciseSO {
    public override Exercise CreateExercise() {
        List<ExerciseMovement> movements = GetMovements();
        var exercise = new BalanceTestExercise(
            backendEnum: BackendEnum,
            category: Category,
            title: Title,
            description: Description,
            requirements: Requirements
        );
        exercise.Movements = movements;
        exercise.RepsAndSetsConfig = RepsAndSetsConfig.CreateConfig();
        
        return exercise;
    }
}