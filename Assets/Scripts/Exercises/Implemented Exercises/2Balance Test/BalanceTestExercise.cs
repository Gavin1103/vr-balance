using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BalanceTestExercise : NormalExercise {
    public float EasyScoreThreshold = 300f;
    public float MediumScoreThreshold = 700f;
    public GameObject npc;
    public BalanceTestExercise(string title, string description, List<string> requirements, List<ExerciseMovement> movements, bool positionNeeded, bool easyDifficulty, bool mediumDifficulty, bool hardDifficulty) : base(title, description, requirements, movements, positionNeeded, easyDifficulty, mediumDifficulty, hardDifficulty) {
    }
    
    public override void StartExercise() {
        base.StartExercise();
        HeightCalibration();
    }

    public void HeightCalibration() {
        // float headHeight = mainCamera.transform.localPosition.y;
        // float scale = defaultHeight / headHeight;
        // transform.localScale = Vector3.one * scale;
        // currentHeadsetPosition = transform.localScale;

        // currentHeadsetPosition.z = mainCamera.transform.localPosition.z;
        // currentHeadsetPosition.x = mainCamera.transform.localPosition.x;
    }

    public override void ExerciseEnded()
    {
        base.ExerciseEnded();
        
        if (npc != null)
        {
            npc.SetActive(false);
        }

        if (ScoreManager.Instance.Score < EasyScoreThreshold)
            DifficultyManager.Instance.SetAdvisedDifficulty(0, Difficulty.Easy);
        else if (ScoreManager.Instance.Score < MediumScoreThreshold)
            DifficultyManager.Instance.SetAdvisedDifficulty(1, Difficulty.Medium);
        else
            DifficultyManager.Instance.SetAdvisedDifficulty(2, Difficulty.Hard);
    }
}