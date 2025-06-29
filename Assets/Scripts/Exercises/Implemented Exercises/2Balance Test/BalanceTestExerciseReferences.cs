using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class BalanceTestExerciseReferences : MonoBehaviour {
    public static BalanceTestExerciseReferences Instance { get; private set; }

    [Header("Saving data")]
    public SaveHeadPositionData saveHeadPositionData;
    public BalanceTestRunner balanceTestRunner;
    [Header("Feedback particles")]
    public ParticleSystem perfectParticles;
    public ParticleSystem greatParticles;
    public ParticleSystem goodParticles;
    public ParticleSystem missParticles;
    [Header("Graph")]
    public GameObject headswayGraph;
    public RectTransform graphContainer;
    public GameObject dotPrefab;


    void Awake() {
        Instance = this;
    }
}