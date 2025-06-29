using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewArcheryExercise", menuName = "Exercise/Archery")]
public class ArcheryExerciseSO : ExerciseSO {
    public override Exercise CreateExercise() {
        ArcheryExercise exercise = new ArcheryExercise(Title, Category, Description, Requirements, Image);
        exercise.Requisites = Requisites;
        return exercise;
    }
}