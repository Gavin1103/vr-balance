using UnityEngine;
using System.Collections;

public class BalanceTestRunner : MonoBehaviour
{
    public ExerciseSequence balanceSequence;
    public NPCExerciseController npcController;
    public GameObject npc;

    private int currentIndex = 0;

    public void StartBalanceTestSequence()
    {
        if (balanceSequence == null || npcController == null)
        {
            Debug.LogError("Missing sequence or controller.");
            return;

           
        }

        if (npc != null)
        {
            npc.SetActive(true);
        }
        currentIndex = 0;
        StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        while (currentIndex < balanceSequence.exercises.Length)
        {
            ExerciseModule module = balanceSequence.exercises[currentIndex];

            Debug.Log($"Starting exercise: {module.exerciseName}");
            npcController.StartExercise(module); // triggers animation override and play

            // Wait at least one frame to allow animation state to start
            yield return null;

            // Wait for the animation duration based on the hold time and reps
            float duration = module.holdTimeAtKeyPoint * module.repetitions + 1f;
            Debug.Log($"Waiting for {duration} seconds");
            yield return new WaitForSeconds(duration);

            Debug.Log($"Finished exercise: {module.exerciseName}");
            currentIndex++;
        }

        if (npc != null)
        {
            npc.SetActive(false);
        }

        Debug.Log("Balance test complete!");
    }
}
