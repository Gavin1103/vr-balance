using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class BalanceTestExercise : GenericExercise
{
    public float EasyScoreThreshold = 300f;
    public float MediumScoreThreshold = 700f;
    public GameObject npc;
    public BalanceTestExercise(
            string title, string description, List<string> requirements,
            List<ExerciseMovement> movements, int amountOfSets, float waitTimeBetweenSets, int amountOfReps, float waitTimeBetweenReps,
            bool positionNeeded, bool easyDifficulty, bool mediumDifficulty, bool hardDifficulty, List<PositionChecker> positionCheckers)
            : base(title, description, requirements, movements, amountOfSets, waitTimeBetweenSets, amountOfReps, waitTimeBetweenReps, positionNeeded, easyDifficulty, mediumDifficulty, hardDifficulty, positionCheckers) {
    }

    public override void StartExercise() {
        refs.RestUI.gameObject.SetActive(false);
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

    public override void ExerciseEnded() {
        base.ExerciseEnded();

        SaveHeadSway();

        if (npc != null) {
            npc.SetActive(false);
        }
    }

    private void SaveHeadSway() {
        // This code expects every movement in the balance test to heave a balance behaviour
        foreach (var movement in Movements) {
            var balanceBehaviour = movement.ExerciseBehaviours.OfType<BalanceBehaviour>().FirstOrDefault();
            ExerciseManager.Instance.saveHeadPositionData.SaveHeadPositionsList(balanceBehaviour.HeadPositions);
        }

        ExerciseManager.Instance.saveHeadPositionData.EndDataCollection();

        foreach (var movement in Movements) {
            var balanceBehaviour = movement.ExerciseBehaviours.OfType<BalanceBehaviour>().FirstOrDefault();
            balanceBehaviour.HeadPositions.Clear();
        }
    }
}