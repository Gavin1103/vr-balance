using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class HeadSwayMovement : IMovementBehaviour
{
    private List<Vector3> headPositions = new List<Vector3>();

    private float holdTime;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float minZ;
    private float maxZ;

    private float positiveZ_Minimum;
    private float positiveZ_Maximum;
    private float negativeZ_Minimum;
    private float negativeZ_Maximum;

    private float positiveX_Minimum;
    private float positiveX_Maximum;
    private float negativeX_Minimum;
    private float negativeX_Maximum;

    private float positiveY_Minimum;
    private float positiveY_Maximum;
    private float restPosition;

    private bool posNeeded;
    private bool easy;
    private bool medium;
    private bool hard;

    public HeadSwayMovement()
    {

    }

    private void Reset()
    {
        // if (exercise is NormalExercise normalExercise) {
        //     normalExercise.RestartCurrentMovement();
        // }
    }

    public override void OnMovementStart(ExerciseMovement movement)
    {
        OnMovementUpdate(movement);
    }

    private Vector3 headsetPos;
    private Vector3 rightArmPos;
    private Vector3 leftArmPos;

    private PositionChecker currentChecker;
    private PositionCheckerSO chosenSO;

    private Vector3 currentHeadsetPos;
    private float minimalPos;
    private float maximalPos;

    public override IEnumerator OnMovementUpdate(ExerciseMovement movement)
    {
        // go back here when resetting

        positiveZ_Minimum = ExerciseManager.Instance.Headset.transform.position.z + minZ;
        positiveZ_Maximum = ExerciseManager.Instance.Headset.transform.position.z + maxZ;
        negativeZ_Minimum = ExerciseManager.Instance.Headset.transform.position.z - minZ;
        negativeZ_Maximum = ExerciseManager.Instance.Headset.transform.position.z - maxZ;

        positiveX_Minimum = ExerciseManager.Instance.Headset.transform.position.x + minX;
        positiveX_Maximum = ExerciseManager.Instance.Headset.transform.position.x + maxX;
        negativeX_Minimum = ExerciseManager.Instance.Headset.transform.position.x - minX;
        negativeX_Maximum = ExerciseManager.Instance.Headset.transform.position.x - maxX;

        positiveY_Minimum = ExerciseManager.Instance.Headset.transform.position.y + minY;
        positiveY_Maximum = ExerciseManager.Instance.Headset.transform.position.y + maxY;

        restPosition = ExerciseManager.Instance.Headset.transform.position.y + 0.03f;

        GenericExerciseReferences.Instance.LeftStickAffordance.SetActive(false);
        GenericExerciseReferences.Instance.RightStickAffordance.SetActive(false);
        GenericExerciseReferences.Instance.HeadsetAffordance.SetActive(false);


        yield return OnMovementUpdate(movement);

        // Hold at target & accumulate score
        float elapsedWhileHolding = 0f;
        while (elapsedWhileHolding < holdTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                elapsedWhileHolding = holdTime;
                break;
            }

            Vector3[] amountPositions = LoadHeight.loadData.ToArray();

            if (amountPositions.Length >= 0)
            {
                headsetPos = amountPositions[0];
                rightArmPos = amountPositions[1];
                leftArmPos = amountPositions[2];
            }



            currentHeadsetPos = ExerciseManager.Instance.Headset.transform.position;

            // get string from chosen difficulty from the difficulty manager
            // DifficultyManager.Instance.SelectedDifficulty = DifficultyManager.Instance.AdvisedDifficulty;
            string chosenDifficulty = DifficultyManager.Instance.SelectedDifficulty.ToString();
            //Debug.Log(chosenDifficulty);

            bool setPosition = GenericExerciseReferences.Instance.NeedsPosition;


            //Debug.Log(posNeeded);

            // Checks if the current exercise difficulty needs position checker
            bool setPositionNeeded = setPosition == true ? true : false;
            // Checks currently chosen difficulty to see if the exercise/minigame needs position check
            bool checkDifficulty = chosenDifficulty == "Easy" ? GenericExerciseReferences.Instance.EasyDifficulty :
                                   chosenDifficulty == "Medium" ? GenericExerciseReferences.Instance.MediumDifficulty :
                                   chosenDifficulty == "Hard" ? GenericExerciseReferences.Instance.HardDifficulty : false;

            PositionChecker currentChecker = GenericExerciseReferences.Instance.currentPosSO;
            Debug.Log("Current Position Checker: " + currentChecker);

            Vector3 minBound = new Vector3(currentChecker.MinX, currentChecker.MinY, currentChecker.MinZ);
            Vector3 maxBound = new Vector3(currentChecker.MaxX, currentChecker.MaxY, currentChecker.MaxZ);

            minimalPos = headsetPos.y - minBound.y;
            maximalPos = headsetPos.y - maxBound.y;

            // Checks to see if the feedback for position check needs to be turned on.
            bool turnOnFeedback = setPositionNeeded == true ? checkDifficulty == true ? true : false : false;



            //Debug.Log("FeedbackCube is turned on: " + turnOnFeedback);
            GenericExerciseReferences.Instance.FeedbackLine.SetActive(turnOnFeedback);
            GenericExerciseReferences.Instance.RenderLineMinimal.SetActive(turnOnFeedback);
            GenericExerciseReferences.Instance.RenderLineMaximal.SetActive(turnOnFeedback);

            Vector3 defaultZ = new Vector3(0, 0, 1);

            GenericExerciseReferences.Instance.RenderLineMinimal.transform.position = (headsetPos + defaultZ) - minBound;
            GenericExerciseReferences.Instance.RenderLineMaximal.transform.position = (headsetPos + defaultZ) - maxBound;
            //Add another feedback stuff using the turnOnFeedback

            if (InBoundsY())
            {
                elapsedWhileHolding += Time.deltaTime;
                GenericExerciseReferences.Instance.HoldMovementText.transform.parent.gameObject.SetActive(true);
                GenericExerciseReferences.Instance.HoldMovementText.text = (holdTime - elapsedWhileHolding).ToString("0.0") + "s";
                GenericExerciseReferences.Instance.InformationObject.SetActive(false);

                RenderCube(Color.green);
            }
            else
            {
                // if exercise has been finished, player cannot be out of bounds

                Reset();
                elapsedWhileHolding = 0;
                GenericExerciseReferences.Instance.HoldMovementText.transform.parent.gameObject.SetActive(false);
                GenericExerciseReferences.Instance.InformationObject.SetActive(true);
                GenericExerciseReferences.Instance.InformationText.text = "Not in bounds!";
            }

            if (elapsedWhileHolding == 0)
            {
                RenderCube(Color.red);
            }

            if (DefaultBound() && elapsedWhileHolding == 0)
            {
                RenderCube(Color.green);
            }
            // Het moet helemaal resetten, dus de image moet weer naar links



            GenericExerciseReferences.Instance.HoldImageLine.sizeDelta = new Vector2(Mathf.Lerp(0, 160, elapsedWhileHolding / holdTime), GenericExerciseReferences.Instance.HoldImageLine.sizeDelta.y);

            yield return null;
        }
        while (Input.GetKey(KeyCode.Space))
        { // Need< Or my Scheiße breaks because the next action image will skip aswell
            yield return null;
        }
        yield return null;
    }

    public override void OnMovementEnd(ExerciseMovement movement)
    {
        GenericExerciseReferences.Instance.LeftStickAffordance.SetActive(true);
        GenericExerciseReferences.Instance.RightStickAffordance.SetActive(true);
        GenericExerciseReferences.Instance.HeadsetAffordance.SetActive(true);

        // Lijn uitzetten
        GenericExerciseReferences.Instance.FeedbackLine.SetActive(false);
        GenericExerciseReferences.Instance.RenderLineMinimal.SetActive(false);
        GenericExerciseReferences.Instance.RenderLineMaximal.SetActive(false);

        GenericExerciseReferences.Instance.InformationObject.SetActive(false);
        GenericExerciseReferences.Instance.HoldMovementText.transform.parent.gameObject.SetActive(false);
        GenericExerciseReferences.Instance.HoldImageLine.sizeDelta = new Vector2(0, GenericExerciseReferences.Instance.HoldImageLine.sizeDelta.y);
    }

    private bool InBoundsX()
    {
        Vector3 headPos = ExerciseManager.Instance.Headset.transform.position;
        if (headPos.x > positiveX_Minimum && headPos.x < positiveX_Maximum ||
            headPos.x < negativeX_Minimum && headPos.x > negativeX_Maximum)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool InBoundsY()
    {
        Debug.Log(headsetPos.y);
        Debug.Log(currentHeadsetPos.y + " BHEHIFBEHBSEHGBSHGSHEGHSBGHSBEGBSGEJ");
        Debug.Log(minimalPos);
        Debug.Log(maximalPos);
        if (currentHeadsetPos.y < minimalPos && currentHeadsetPos.y > maximalPos)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool InBoundsZ()
    {
        Vector3 headPos = ExerciseManager.Instance.Headset.transform.position;
        if (headPos.z > positiveZ_Minimum && headPos.z < positiveZ_Maximum ||
            headPos.z < negativeZ_Minimum && headPos.z > negativeZ_Maximum)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool DefaultBound()
    {
        Vector3 headPos = ExerciseManager.Instance.Headset.transform.position;
        if (headPos.y > restPosition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RenderCube(Color name)
    {
        GenericExerciseReferences.Instance.FeedbackLine.GetComponent<Renderer>().material.color = name;
    }
}
