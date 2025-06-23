using System.Collections.Generic;
using UnityEngine;

public class RepsAndSetsConfig {
    [Header("Easy")]
    public int EasyReps = 5;
    public int EasySets = 1;
    public float EasyWaitBetweenReps = 0.5f;
    public float EasyWaitBetweenSets = 10f;

    [Header("Medium")]
    public int MediumReps = 8;
    public int MediumSets = 2;
    public float MediumWaitBetweenReps = 0.4f;
    public float MediumWaitBetweenSets = 8f;

    [Header("Hard")]
    public int HardReps = 12;
    public int HardSets = 3;
    public float HardWaitBetweenReps = 0.3f;
    public float HardWaitBetweenSets = 6f;
    
    public RepsAndSetsConfig(RepsAndSetsSO repsAndSetsSO) {
        EasyReps = repsAndSetsSO.EasyReps;
        EasySets = repsAndSetsSO.EasySets;
        EasyWaitBetweenReps = repsAndSetsSO.EasyWaitBetweenReps;
        EasyWaitBetweenSets = repsAndSetsSO.EasyWaitBetweenSets;

        MediumReps = repsAndSetsSO.MediumReps;
        MediumSets = repsAndSetsSO.MediumSets;
        MediumWaitBetweenReps = repsAndSetsSO.MediumWaitBetweenReps;
        MediumWaitBetweenSets = repsAndSetsSO.MediumWaitBetweenSets;

        HardReps = repsAndSetsSO.HardReps;
        HardSets = repsAndSetsSO.HardSets;
        HardWaitBetweenReps = repsAndSetsSO.HardWaitBetweenReps;
        HardWaitBetweenSets = repsAndSetsSO.HardWaitBetweenSets;
    }

    public void ApplyTo(GenericExercise exercise) {
        var difficulty = DifficultyManager.Instance.SelectedDifficulty;

        switch (difficulty) {
            case Difficulty.Easy:
                exercise.AmountOfReps = EasyReps;
                exercise.AmountOfSets = EasySets;
                exercise.WaitTimeBetweenReps = EasyWaitBetweenReps;
                exercise.WaitTimeBetweenSets = EasyWaitBetweenSets;
                break;
            case Difficulty.Medium:
                exercise.AmountOfReps = MediumReps;
                exercise.AmountOfSets = MediumSets;
                exercise.WaitTimeBetweenReps = MediumWaitBetweenReps;
                exercise.WaitTimeBetweenSets = MediumWaitBetweenSets;
                break;
            case Difficulty.Hard:
                exercise.AmountOfReps = HardReps;
                exercise.AmountOfSets = HardSets;
                exercise.WaitTimeBetweenReps = HardWaitBetweenReps;
                exercise.WaitTimeBetweenSets = HardWaitBetweenSets;
                break;
        }
    }
}