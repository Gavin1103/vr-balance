using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Models.DTO.Exercise;
using Models.User;
using Service;
using System;

public class FireFlyExercise : Exercise {
    public FireFlyExercise(string title, string description, List<string> requirements, bool positionNeeded, bool easyDifficulty, bool mediumDifficulty, bool hardDifficulty) : base(title, description, requirements, positionNeeded, easyDifficulty, mediumDifficulty, hardDifficulty) {
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