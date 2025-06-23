using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ArcheryExercise : Exercise {
    
    public ArcheryExercise(string title, ExerciseCategory category, string description, List<string> requirements, bool positionNeeded, bool easyDifficulty, bool mediumDifficulty, bool hardDifficulty, List<PositionChecker> positionCheckers) : base(title, category, description, requirements, positionNeeded, easyDifficulty, mediumDifficulty, hardDifficulty, positionCheckers) {
    }

    // Reimplement the necessary methods
}
