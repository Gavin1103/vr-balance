using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class Exercise {
    public string Title;
    public ExerciseCategory Category;
    public string Description;
    public List<string> Requirements;

    public bool PosNeeded;
    public bool Easy;
    public bool Medium;
    public bool Hard;

    public List<PositionChecker> PositionCheckers;

    protected Difficulty dif {
        get {
            return DifficultyManager.Instance.SelectedDifficulty;
        }
    }
    protected Exercise(string title, ExerciseCategory category, string description, List<string> requirements, bool positionNeeded, bool easyDifficulty, bool mediumDifficulty, bool hardDifficulty, List<PositionChecker> positionChecker)
    {
        Title = title;
        Category = category;
        Description = description;
        Requirements = requirements;

        PosNeeded = positionNeeded;
        Easy = easyDifficulty;
        Medium = hardDifficulty;
        Hard = hardDifficulty;

        PositionCheckers = positionChecker;
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
    
    public virtual void DisplayEndScreen() {
        // Called from ExerciseManager after ExerciseEnded() is called
        string[] messages = {
            $"Great job completing the {Title}!",
            $"You’ve finished the {Title} – well done!",
            $"That’s it for the {Title}. Nice work!",
            $"The {Title} is complete – keep it up!",
            $"Well done! {Title} finished successfully.",
            $"You just wrapped up the {Title} – awesome!",
            $"That was the {Title}. Great effort!",
            $"Nice! {Title} completed and logged."
        };
        string selectedMessage = messages[Random.Range(0, messages.Length)];
        EndScreenUI.Instance.UpdateEndUI(selectedMessage, "Score:", Mathf.RoundToInt(ScoreManager.Instance.Score).ToString());
    }
}