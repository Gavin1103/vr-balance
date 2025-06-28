using UnityEngine;
using System.Collections;
public class NPCExerciseController : MonoBehaviour
{
    [Header("Animator Setup")]
    public Animator animator;
    public AnimatorOverrideController overrideController;
    public AudioSource npcAudioSource; 


    void Start()
    {
        animator.speed = 0; // Pause any automatic playback
    }
    public void StartExercise(ExerciseModule module)
    {
        if (module == null || animator == null || module.npcAnimation == null)
        {
            Debug.LogWarning("Missing module, animator, or animation.");
            return;
        }

        if (module.instructionAudio != null && npcAudioSource != null)
        {
            npcAudioSource.PlayOneShot(module.instructionAudio);
        }

        // Resume animation in case it was paused
        animator.speed = 1;

        overrideController["Breathing_Idle"] = module.npcAnimation;
        animator.runtimeAnimatorController = overrideController;
        animator.Rebind();
        animator.Update(0f);
        animator.Play("DefaultState", 0, 0f);

        //StartCoroutine(PerformExercise(module));
    }


    private IEnumerator PerformExercise(ExerciseModule module)
    {
        Debug.Log($"Starting exercise: {module.exerciseName}");

        // Make sure animator is running
        animator.speed = 1;
        animator.Play("DefaultState", 0, 0f);

        // Wait until key pose is visually reached
        float timeToPose = module.keyPoseFrame / module.frameRate;
        Debug.Log($"Waiting {timeToPose:F2}s to reach key pose");
        yield return new WaitForSeconds(timeToPose);

        // Pause the animation exactly on the pose
        animator.speed = 0;
        Debug.Log($"Paused on pose for: {module.exerciseName}");

        // Hold the pose for the user to mirror
        yield return new WaitForSeconds(module.holdTimeAtKeyPoint);

        // Resume animation to finish movement
        animator.speed = 1;

        // Calculate time remaining to finish the full animation
        float totalClipLength = module.npcAnimation.length;
        float timeRemaining = totalClipLength - timeToPose;
        Debug.Log($"Finishing animation for {timeRemaining:F2}s");
        yield return new WaitForSeconds(timeRemaining);

        Debug.Log($"Finished exercise: {module.exerciseName}");
    }



}


