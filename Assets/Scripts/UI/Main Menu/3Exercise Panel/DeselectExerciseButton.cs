using UnityEngine;
using UnityEngine.UI;

public class DeselectExerciseButton : MonoBehaviour
{
    public void DeselectExercise()
    {
        ExerciseManager.Instance.SelectedExerciseInfo.SetActive(false);
        ExerciseManager.Instance.Leaderboard.SetActive(false);
        ExerciseManager.Instance.ExercisesMenu.SetActive(true);
    }
}