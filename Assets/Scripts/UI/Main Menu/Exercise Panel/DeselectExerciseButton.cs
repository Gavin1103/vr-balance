using UnityEngine;
using UnityEngine.UI;

public class DeselectExerciseButton : MonoBehaviour
{
    public void DeselectExercise()
    {
        // Hide the selected exercise info panel
        ExerciseManager.Instance.SelectedExerciseInfo.SetActive(false);
        // Show the exercises menu again
        ExerciseManager.Instance.ExercisesMenu.SetActive(true);
    }
}