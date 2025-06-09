using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewArcheryExercise", menuName = "Exercise/Exercises/Archery")]
public class ArcheryExerciseSO : ExerciseSO {
    public override Exercise CreateExercise() {
        return new ArcheryExercise(Title, Description, Requirements, PositionNeeded, EasyDifficulty, MediumDifficulty, HardDifficulty);
    }
}