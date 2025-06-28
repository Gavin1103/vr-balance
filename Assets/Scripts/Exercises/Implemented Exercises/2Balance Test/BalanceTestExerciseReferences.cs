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

    void Awake() {
        Instance = this;
    }
}