using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class Exercise {
    public string Title;
    public string Description;
    public List<string> Requirements;

    public bool PosNeeded;
    public bool Easy;
    public bool Medium;
    public bool Hard;

    protected Difficulty dif {
        get {
            return DifficultyManager.Instance.SelectedDifficulty;
        }
    }
    protected Exercise(string title, string description, List<string> requirements, bool positionNeeded, bool easyDifficulty, bool mediumDifficulty, bool hardDifficulty) {
        Title = title;
        Description = description;
        Requirements = requirements;

        PosNeeded = positionNeeded;
        Easy = easyDifficulty;
        Medium = hardDifficulty;
        Hard = hardDifficulty;
    }

    public virtual void StartExercise() {
        // Called from ExerciseManager
        PlayExercise();
    }

    protected virtual void PlayExercise() {
        // When inheritting, implement exercise logic here
    }

    public virtual void ExerciseEnded() {
        // Called from ExerciseManager
    }
}