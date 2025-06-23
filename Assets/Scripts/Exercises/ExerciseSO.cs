using UnityEngine;
using System.Collections.Generic;

public abstract class ExerciseSO : ScriptableObject
{
    public string Title;
    public ExerciseCategory Category;
    public string Description;
    public List<string> Requirements;

    public bool PositionNeeded;
    public bool EasyDifficulty;
    public bool MediumDifficulty;
    public bool HardDifficulty;

    public List<PositionChecker> PositionChecker;

    public abstract Exercise CreateExercise();
}