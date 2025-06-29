using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewExerciseRequisites", menuName = "Requisites")]
public class ExerciseRequisitesSO : ScriptableObject {
    public bool RequiresBalanceTest = true;
    public bool RequiresCalibration = false;
    public ExerciseRequisitesSO() {
        
    }
}