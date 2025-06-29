using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewFruitWarriorExercise", menuName = "Exercise/Fruit Warrior")]
public class FruitWarriorExerciseSO : ExerciseSO {
    public override Exercise CreateExercise() {
        return new FruitWarriorExercise(Title, Category, Description, Requirements, Image);
    }
}