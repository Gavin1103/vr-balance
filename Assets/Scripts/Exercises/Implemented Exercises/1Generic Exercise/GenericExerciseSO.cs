using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewGenericExercise", menuName = "Exercise/Exercises/Generic")]
public class GenericExerciseSO : ExerciseSO {
    public string BackendEnum;

    public RepsAndSetsSO RepsAndSetsConfig;
    public List<ExerciseMovementSO> Movements;
    public List<PositionCheckerSO> CheckPositions;

    void OnEnable() {
        RepsAndSetsConfig = ScriptableObject.CreateInstance<RepsAndSetsSO>();
    }
    public override Exercise CreateExercise() {
        List<ExerciseMovement> movements = GetMovements();
        var exercise = new GenericExercise(
            backendEnum: BackendEnum,
            title: Title,
            description: Description,
            requirements: Requirements,
            positionNeeded: PositionNeeded,
            easyDifficulty: EasyDifficulty,
            mediumDifficulty: MediumDifficulty,
            hardDifficulty: HardDifficulty,
            positionCheckers: GetPositionCheckers()
        );
        exercise.Movements = movements;
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

    protected List<PositionChecker> GetPositionCheckers() {
        var list = new List<PositionChecker>();
        foreach (var so in CheckPositions) {
            list.Add(so.SetPosition());
        }
        return list;
    }
}