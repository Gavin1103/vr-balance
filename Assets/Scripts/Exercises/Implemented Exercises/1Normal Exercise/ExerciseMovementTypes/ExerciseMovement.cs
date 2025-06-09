using System.Collections;
using UnityEngine;

public abstract class ExerciseMovement {
    [HideInInspector] public NormalExercise exercise;

    public float Duration { get; set; }
    public Sprite InstructionImage { get; set; }
    public float TotalScore { get; set; }

    // Start & end of the instructionimages
    protected Vector3 startPos = new Vector3(400, 0, 0);
    protected Vector3 endPos = new Vector3(-400, 0, 0);
    // Amount of score gathered for this movement
    protected float currentScore;


    protected ExerciseMovement(float duration, Sprite image, float score) {
        Duration = duration;
        InstructionImage = image;
        TotalScore = score;
    }

    public virtual IEnumerator Play() {
        // Move instruction image to the end
        float elapsed = 0f;
        while (elapsed < Duration) {
            NormalExerciseReferences.Instance.ActionImageLine.sizeDelta = new Vector2(Mathf.Lerp(0, 160, elapsed / Duration), NormalExerciseReferences.Instance.ActionImageLine.sizeDelta.y);
            NormalExerciseReferences.Instance.MovementImageObject.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / Duration);
            elapsed += Time.deltaTime;

            // for debugging
            if (Input.GetKeyDown(KeyCode.Space)) {
                elapsed = Duration;
                NormalExerciseReferences.Instance.ActionImageLine.sizeDelta = new Vector2(160, NormalExerciseReferences.Instance.ActionImageLine.sizeDelta.y);
                NormalExerciseReferences.Instance.MovementImageObject.transform.localPosition = endPos;
                break;
            }

            yield return null;
        }
        while (Input.GetKey(KeyCode.Space)) { // Need< Or my Scheiße breaks because the next action image will skip aswell
            yield return null;
        }
    }

    public abstract void MovementEnded();
}