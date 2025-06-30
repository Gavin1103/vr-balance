using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Video;

public abstract class ExerciseSO : ScriptableObject {
    public string Title;
    public ExerciseCategory Category;
    public string Description;
    public List<string> Requirements;
    public Sprite Image;
    public ExerciseRequisitesSO Requisites;
    public VideoClip ExplanationVideo;

    public abstract Exercise CreateExercise();
}