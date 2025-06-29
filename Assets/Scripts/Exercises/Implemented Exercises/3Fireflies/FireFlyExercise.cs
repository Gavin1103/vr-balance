using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Service;

public class FireFlyExercise : Exercise {
    public FireFlyExercise(string title, ExerciseCategory category, string description, List<string> requirements, Sprite image) : base(title, category, description, requirements, image) {
    }

    public override void StartExercise() {
        FireFlyWaveManager.FireFlyInstance.StartWaves();
        // SoundManager.Instance.PlayMusic("BackgroundMusic");
        // SoundManager.Instance.PlayAmbience("ForestAmbience");
    }

    public override void ExerciseEnded() {
        FireFlyWaveManager.FireFlyInstance.EndSession();
        //FireFlyWaveManager.FireFlyInstance.Gavin();
    }
}