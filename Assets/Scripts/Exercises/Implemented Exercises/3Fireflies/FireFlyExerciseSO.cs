using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewFireFlyExercise", menuName = "Exercise/Exercises/FireFly")]
public class FireFlyExerciseSO : ExerciseSO {
    public override Exercise CreateExercise() {
        return new FireFlyExercise(Title, Category, Description, Requirements);
    }
}