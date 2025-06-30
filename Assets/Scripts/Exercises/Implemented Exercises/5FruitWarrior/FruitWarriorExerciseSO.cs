using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewFruitWarriorExercise", menuName = "Exercise/Fruit Warrior")]
public class FruitWarriorExerciseSO : ExerciseSO {
    public override Exercise CreateExercise() {
        FruitWarriorExercise exercise = new FruitWarriorExercise(Title, Category, Description, Requirements, Image);
        exercise.Requisites = Requisites;
        exercise.ExplanationVideo = ExplanationVideo;
        return exercise;
    }
}