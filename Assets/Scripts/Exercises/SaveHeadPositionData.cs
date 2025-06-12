using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DTO.Request.Exercise;
using Exercises.Implemented_Exercises._2Balance_Test;
using NUnit.Framework;
using Unity.XR.GoogleVr;
using UnityEditor;
using UnityEngine;

public class SaveHeadPositionData : MonoBehaviour
{
    List<List<Vector3>> AllPhasePositions = new List<List<Vector3>>();
    ExerciseService exerciseService;
    HeadSway headSway;
    
    void Start()
    {
        exerciseService = new ExerciseService();
        headSway = new HeadSway();
    }
    
    public void SaveHeadPositionsList(List<Vector3> headPositions)
    {
        AllPhasePositions.Add(headPositions);
    }

    public void EndDataCollection()
    {
        CompletedBalanceTestExerciseDTO dto = new CompletedBalanceTestExerciseDTO
        {
            exercise = "Balance",
            completedAt = DateTime.UtcNow,
            difficulty = Difficulty.None,
            earnedPoints = 0,
            phase_1 = AllPhasePositions[0].Select(v => new Vector3DTO(v)).ToList(),
            phase_2 = AllPhasePositions[1].Select(v => new Vector3DTO(v)).ToList(),
            phase_3 = AllPhasePositions[2].Select(v => new Vector3DTO(v)).ToList(),
            phase_4 = AllPhasePositions[3].Select(v => new Vector3DTO(v)).ToList()
        };

        headSway.DetermineDifficultyFromFirstTwoPhases(dto.phase_1, dto.phase_2);
        
        StartCoroutine(exerciseService.SaveExercise(
            dto,
            onSuccess: ApiResponse => { Debug.Log(ApiResponse.message); },
            onError: error => { Debug.Log(error.message); },
            "balance-test"
        ));       
        
        AllPhasePositions.Clear();
    }
}