using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Service;

public class FireFlyExercise : Exercise {
    public FireFlyExercise(string title, ExerciseCategory category, string description, List<string> requirements, bool positionNeeded, bool easyDifficulty, bool mediumDifficulty, bool hardDifficulty, List<PositionChecker> positionCheckers) : base(title, category, description, requirements, positionNeeded, easyDifficulty, mediumDifficulty, hardDifficulty, positionCheckers) {
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