using System.Collections.Generic;
using UnityEngine;

public class ExerciseFactory : MonoBehaviour {
    public static ExerciseFactory Instance { get; private set; }

    void Awake() {
        Instance = this;
    }

    public static List<Exercise> CreateAllExercises() {
        List<Exercise> exercises = new List<Exercise>();

        ExerciseSO[] exerciseAssets = Resources.LoadAll<ExerciseSO>("Exercises");

        // Load all exercises
        foreach (ExerciseSO exerciseAsset in exerciseAssets) {
            Exercise exercise = exerciseAsset.CreateExercise();
            exercises.Add(exercise);
        }

        return exercises;
    }
}