using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class Exercise {
    public string Title;
    public ExerciseCategory Category;
    public string Description;
    public List<string> Requirements;
    public Sprite Image;

    protected Difficulty dif {
        get {
            return DifficultyManager.Instance.SelectedDifficulty;
        }
    }
    protected Exercise(string title, ExerciseCategory category, string description, List<string> requirements, Sprite image) {
        Title = title;
        Category = category;
        Description = description;
        Requirements = requirements;
        Image = image;
    }

    public virtual void StartExercise() {
        // Called from ExerciseManager
    }

    public virtual void PlayExercise() {
        // When inheritting, implement exercise logic here. This is called every frame by the ExerciseManager
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