using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewGenericExercise", menuName = "Exercise/Exercises/Generic")]
public class GenericExerciseSO : ExerciseSO {
    public List<ExerciseMovementSO> Movements;
    [Min(1)]
    public int AmountOfSets = 1;
    public float WaitTimeBetweenSets = 10f;
    [Min(1)]
    public int AmountOfReps = 1;
    public float WaitTimeBetweenReps = 0.5f;
    
    public List<PositionCheckerSO> CheckPositions;

    public List<ExerciseMovement> GetMovements() {
        List<ExerciseMovement> movements = new List<ExerciseMovement>();
        foreach (var moveSO in Movements) {
            ExerciseMovement movement = moveSO.CreateMovement();
            movements.Add(movement);
        }
        return movements;
    }
    public List<PositionChecker> GetPositionCheckers() {
        List<PositionChecker> positionCheckers = new List<PositionChecker>();
        foreach (var PosDifficultySO in CheckPositions) {
            PositionChecker positionCheck = PosDifficultySO.SetPosition();
            positionCheckers.Add(positionCheck);
        }
        return positionCheckers;
    }
    public override Exercise CreateExercise() {
        List<ExerciseMovement> movements = GetMovements();
        List<PositionChecker> positionCheckers = GetPositionCheckers();

        return new GenericExercise(Title, Description, Requirements, movements, AmountOfSets, WaitTimeBetweenSets, AmountOfReps, WaitTimeBetweenReps, PositionNeeded, EasyDifficulty, MediumDifficulty, HardDifficulty, positionCheckers);
    }
}