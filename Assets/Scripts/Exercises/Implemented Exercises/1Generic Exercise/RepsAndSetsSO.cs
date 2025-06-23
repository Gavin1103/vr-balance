using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RepsAndSets", menuName = "Exercise/Reps & Sets")]
public class RepsAndSetsSO : ScriptableObject
{
    [Header("Easy")]
    public int EasyReps = 5;
    public int EasySets = 1;
    public float EasyWaitBetweenReps = 0.5f;
    public float EasyWaitBetweenSets = 10f;

    [Header("Medium")]
    public int MediumReps = 8;
    public int MediumSets = 2;
    public float MediumWaitBetweenReps = 0.4f;
    public float MediumWaitBetweenSets = 8f;

    [Header("Hard")]
    public int HardReps = 12;
    public int HardSets = 3;
    public float HardWaitBetweenReps = 0.3f;
    public float HardWaitBetweenSets = 6f;
    
    public RepsAndSetsConfig CreateConfig() {
        RepsAndSetsConfig config = new RepsAndSetsConfig(this);
        return config;
    }
}