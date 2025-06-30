using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewFireFlyExercise", menuName = "Exercise/FireFly")]
public class FireFlyExerciseSO : ExerciseSO {
    public override Exercise CreateExercise() {
        FireFlyExercise exercise = new FireFlyExercise(Title, Category, Description, Requirements, Image);
        exercise.Requisites = Requisites;
        exercise.ExplanationVideo = ExplanationVideo;
        return exercise;
    }
}