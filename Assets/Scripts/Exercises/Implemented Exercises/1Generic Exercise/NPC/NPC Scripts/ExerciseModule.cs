using UnityEngine;

[CreateAssetMenu(fileName = "NewExerciseModule", menuName = "Physio/Exercise Module")]
public class ExerciseModule : ScriptableObject
{
    [Header("Pose Timing")]
    public int keyPoseFrame = 0;
    public float frameRate = 30f;

    public string exerciseName;
    [TextArea] public string description;

    public AnimationClip npcAnimation;
    public float holdTimeAtKeyPoint = 10f;
    public int repetitions = 1;

    public AudioClip instructionAudio;
    public Sprite exerciseIcon;
}