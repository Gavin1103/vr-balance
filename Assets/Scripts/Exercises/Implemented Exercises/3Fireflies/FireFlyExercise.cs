using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Service;

public class FireFlyExercise : Exercise {
    public FireFlyExercise(string title, ExerciseCategory category, string description, List<string> requirements) : base(title, category, description, requirements) {
    }

    protected override void PlayExercise() {
        FireFlyWaveManager.FireFlyInstance.StartWaves();
    }

    public override void ExerciseEnded() {
        FireFlyWaveManager.FireFlyInstance.EndSession();
        //FireFlyWaveManager.FireFlyInstance.Gavin();
        ScoreManager.Instance.ResetScore();
    }
}