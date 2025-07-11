using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewGenericExercise", menuName = "Exercise/Generic/Generic")]
public class GenericExerciseSO : ExerciseSO {
    public string BackendEnum;

    public RepsAndSetsSO RepsAndSetsConfig;
    public List<ExerciseMovementSO> Movements;

    public override Exercise CreateExercise() {
        List<ExerciseMovement> movements = GetMovements();
        var exercise = new GenericExercise(
            backendEnum: BackendEnum,
            category: Category,
            title: Title,
            description: Description,
            requirements: Requirements,
            image: Image
        );
        exercise.Movements = movements;
        exercise.Requisites = Requisites;
        exercise.ExplanationVideo = ExplanationVideo;
        exercise.RepsAndSetsConfig = RepsAndSetsConfig.CreateConfig();

        return exercise;
    }
    
    protected List<ExerciseMovement> GetMovements() {
        List<ExerciseMovement> movements = new List<ExerciseMovement>();
        foreach (var moveSO in Movements) {
            ExerciseMovement movement = moveSO.CreateMovement();
            movements.Add(movement);
        }
        return movements;
    }
}