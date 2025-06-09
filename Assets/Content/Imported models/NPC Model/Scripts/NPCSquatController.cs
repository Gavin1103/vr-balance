using UnityEngine;
using TMPro;

public class NPCSquatController : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;
    public string animationStateName = "AirSquat";  // Your squat animation name
    [Range(0f, 1f)]
    public float pauseAtNormalizedTime = 0.47f;     // Where to pause (e.g. frame 40 of 85 = ~0.47)

    [Header("VR Transforms")]
    public Transform headTransform;
    public Transform leftHandTransform;
    public Transform rightHandTransform;

    [Header("Squat Settings")]
    public float squatThreshold = 0.5f; // Difference in Y between head and average hands
    public float holdDuration = 10f;    // How long to hold before continuing

    public TextMeshProUGUI timerText; // Drag and drop your UI element in the Inspector

    private float holdTimer = 0f;
    private bool hasPaused = false;
    private bool resumedAnimation = false;

    void Update()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        if (currentState.IsName(animationStateName))
        {
            float normalizedTime = currentState.normalizedTime % 1f;

            // Pause the animation
            if (!hasPaused && !resumedAnimation && normalizedTime >= pauseAtNormalizedTime)
            {
                Debug.Log("Pausing animation at squat hold.");
                animator.speed = 0f;
                hasPaused = true;
            }

            // If paused, start checking squat and timer
            if (hasPaused)
            {
                float headY = headTransform.position.y;
                float leftY = leftHandTransform.position.y;
                float rightY = rightHandTransform.position.y;
                float avgHandY = (leftY + rightY) / 2f;
                float distance = headY - avgHandY;

                if (distance > squatThreshold)
                {
                    holdTimer += Time.deltaTime;
                    timerText.text = Mathf.CeilToInt(holdTimer).ToString();
                    Debug.Log($"Holding squat: {holdTimer:F2}s");

                    if (holdTimer >= holdDuration)
                    {
                        Debug.Log("Squat held long enough, resuming animation.");

                        animator.speed = 1f;
                        float resumePoint = Mathf.Min(pauseAtNormalizedTime + 0.1f, 0.95f);
                        animator.Play(animationStateName, 0, resumePoint);

                        hasPaused = false;
                        resumedAnimation = true;
                        holdTimer = 0f;
                    }
                }
                else
                {
                    // Reset timer if user moves out of squat
                    if (holdTimer > 0f)
                    holdTimer = 0f;
                    timerText.text = "";
                    Debug.Log("Squat interrupted. Resetting timer.");
                }
            }
        }
        else
        {
            // Reset everything if animation state is not active
            resumedAnimation = false;
            holdTimer = 0f;
        }
    }
}
