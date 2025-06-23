using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class BalanceTestExercise : GenericExercise
{

    public BalanceTestExercise(string backendEnum, ExerciseCategory category, string title, string description, List<string> requirements,
            bool positionNeeded,
            bool easyDifficulty,
            bool mediumDifficulty,
            bool hardDifficulty,
            List<PositionChecker> positionCheckers)
            : base(backendEnum, title, category, description, requirements, positionNeeded, easyDifficulty, mediumDifficulty, hardDifficulty, positionCheckers) {
        AmountOfSets = 1;
        AmountOfReps = 1;
        WaitTimeBetweenSets = 0;
        WaitTimeBetweenReps = 0;
    }

    public override void StartExercise() {
        base.StartExercise();
        refs.RepsAndSetsObject.SetActive(false);
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
    }
    
    public override void DisplayEndScreen() {
        EndScreenUI.Instance.UpdateEndUI("Your headsway has been calculated", "Advised difficulty:", DifficultyManager.Instance.AdvisedDifficulty.ToString());
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

    protected override void SaveExercise() {
        return; // No need to save the balance test exercise as it is already saved in SaveHeadPositionData
    }
}