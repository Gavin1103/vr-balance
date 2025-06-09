using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseMovement {
    [HideInInspector] public GenericExercise exercise;

    public List<IMovementBehaviour> ExerciseBehaviours { get; set; }
    public float Duration { get; set; }
    public Sprite InstructionImage { get; set; }
    public float TotalScore { get; set; }

    public Vector3 LeftStickTarget;
    public Vector3 RightStickTarget;
    public Vector3 HeadTarget;

    public Vector3 startPos = new Vector3(400, 0, 0);
    public Vector3 endPos = new Vector3(-400, 0, 0);
    public float currentScore;
    
    
    public ExerciseMovement(Vector3 left, Vector3 right, Vector3 head, float duration, Sprite image, float score, List<IMovementBehaviour> behaviours) {
        LeftStickTarget = left;
        RightStickTarget = right;
        HeadTarget = head;

        ExerciseBehaviours = behaviours;

        Duration = duration;
        InstructionImage = image;
        TotalScore = score;
    }

    public IEnumerator Play() {
        foreach (IMovementBehaviour behaviour in ExerciseBehaviours) {
            behaviour.ExerciseMovement = this;
            behaviour.OnMovementStart(this);
        }

        // Move instruction image to the end
        float elapsed = 0f;
        while (elapsed < Duration)
        {
            GenericExerciseReferences.Instance.ActionImageLine.sizeDelta = new Vector2(Mathf.Lerp(0, 160, elapsed / Duration), GenericExerciseReferences.Instance.ActionImageLine.sizeDelta.y);
            GenericExerciseReferences.Instance.MovementImageObject.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / Duration);
            elapsed += Time.deltaTime;

            // for debugging
            if (Input.GetKeyDown(KeyCode.Space))
            {
                elapsed = Duration;
                GenericExerciseReferences.Instance.ActionImageLine.sizeDelta = new Vector2(160, GenericExerciseReferences.Instance.ActionImageLine.sizeDelta.y);
                GenericExerciseReferences.Instance.MovementImageObject.transform.localPosition = endPos;
                break;
            }

            yield return null;
        }
        while (Input.GetKey(KeyCode.Space))
        { // Need< Or my Scheiße breaks because the next action image will skip aswell
            yield return null;
        }
        
        // Wait for all behaviours to complete their update
        foreach (IMovementBehaviour behaviour in ExerciseBehaviours) {
            yield return behaviour.OnMovementUpdate(this);
        }
    }

    public void MovementEnded() {
        foreach (IMovementBehaviour behaviour in ExerciseBehaviours) {
            behaviour.OnMovementEnd(this);
        }
    }
}