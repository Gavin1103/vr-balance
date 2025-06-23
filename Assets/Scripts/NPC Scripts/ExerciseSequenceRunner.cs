using UnityEngine;

public class ExerciseSequenceRunner : MonoBehaviour
{
    public ExerciseSequence sequence;
    public NPCExerciseController npcController;

    //private int currentIndex = 0;

    void Start()
    {
        if (sequence == null || npcController == null)
        {
            Debug.LogError("Sequence or NPC controller not assigned.");
            return;
        }

        StartCoroutine(RunSequence());
    }

    private System.Collections.IEnumerator RunSequence()
    {
        foreach (var module in sequence.exercises)
        {
            npcController.StartExercise(module);

            // Wait for the module's duration (could refine this to sync with actual animation length)
            float wait = module.holdTimeAtKeyPoint * module.repetitions + 0.5f;
            yield return new WaitForSeconds(wait);
        }

        Debug.Log("Exercise sequence complete.");
    }
}
