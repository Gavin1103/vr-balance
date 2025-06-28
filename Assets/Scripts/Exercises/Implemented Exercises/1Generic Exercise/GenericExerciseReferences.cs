using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class GenericExerciseReferences : MonoBehaviour
{
    public static GenericExerciseReferences Instance { get; private set; }

    [Header("Affordances")]
    public GameObject LeftStickAffordance;
    public GameObject RightStickAffordance;
    public GameObject HeadsetAffordance;
    public AffordancePulse LeftPulseAffordance;
    public AffordancePulse RightPulseAffordance;
    public AffordancePulse HeadsetPulseAffordance;
    public TextMeshProUGUI LeftTextPercentage;
    public TextMeshProUGUI RightTextPercentage;
    public TextMeshProUGUI HeadTextPercentage;
    [Header("UI References")]
    public GameObject SequenceUI;
    public GameObject MovementImageObject;
    public RectTransform ActionImageLine;
    [Header("Reps & Sets References")]
    public GameObject RepsAndSetsObject;
    public TextMeshProUGUI RepsAndSetsText;
    [Header("Behaviour Specific References")]
    public TextMeshProUGUI HoldMovementText;
    public RectTransform HoldImageLine;
    public GameObject InformationObject;
    public TextMeshProUGUI InformationText;
    public GameObject EyesClosedSphere;

    [Header("Feedback for exercises that needs specific positions")]
    public GameObject FeedbackLine;
    public GameObject RenderLineMinimal;
    public GameObject RenderLineMaximal;

    void Awake()
    {
        Instance = this;
    }
}