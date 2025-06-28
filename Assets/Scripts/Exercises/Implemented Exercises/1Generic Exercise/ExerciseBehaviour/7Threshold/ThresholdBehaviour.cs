using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ThresholdBehaviour : IMovementBehaviour
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

    private float EasyHoldTime;
    private float EasyMinX;
    private float EasyMaxX;
    private float EasyMinY;
    private float EasyMaxY;
    private float EasyMinZ;
    private float EasyMaxZ;

    private float MediumHoldTime;
    private float MediumMinX;
    private float MediumMaxX;
    private float MediumMinY;
    private float MediumMaxY;
    private float MediumMinZ;
    private float MediumMaxZ;

    private float HardHoldTime;
    private float HardMinX;
    private float HardMaxX;
    private float HardMinY;
    private float HardMaxY;
    private float HardMinZ;
    private float HardMaxZ;

    public ThresholdBehaviour() {

    }

    public void SetDifficultyVariables(
        float easyHold, float easyMinX, float easyMaxX, float easyMinY, float easyMaxY, float easyMinZ, float easyMaxZ,
        float medHold, float medMinX, float medMaxX, float medMinY, float medMaxY, float medMinZ, float medMaxZ,
        float hardHold, float hardMinX, float hardMaxX, float hardMinY, float hardMaxY, float hardMinZ, float hardMaxZ)
    {
        this.EasyHoldTime = easyHold;
        this.EasyMinX = easyMinX;
        this.EasyMaxX = easyMaxX;
        this.EasyMinY = easyMinY;
        this.EasyMaxY = easyMaxY;
        this.EasyMinZ = easyMinZ;
        this.EasyMaxZ = easyMaxZ;

        this.MediumHoldTime = medHold;
        this.MediumMinX = medMinX;
        this.MediumMaxX = medMaxX;
        this.MediumMinY = medMinY;
        this.MediumMaxY = medMaxY;
        this.MediumMinZ = medMinZ;
        this.MediumMaxZ = medMaxZ;

        this.HardHoldTime = hardHold;
        this.HardMinX = hardMinX;
        this.HardMaxX = hardMaxX;
        this.HardMinY = hardMinY;
        this.HardMaxY = hardMaxY;
        this.HardMinZ = hardMinZ;
        this.HardMaxZ = hardMaxZ;
    }

    public override void OnMovementStart(ExerciseMovement movement) {
        Difficulty diff = DifficultyManager.Instance.SelectedDifficulty;
        if (diff == Difficulty.Easy) {
            holdTime = EasyHoldTime;
            minX = EasyMinX; maxX = EasyMaxX;
            minY = EasyMinY; maxY = EasyMaxY;
            minZ = EasyMinZ; maxZ = EasyMaxZ;
        } else if (diff == Difficulty.Medium) {
            holdTime = MediumHoldTime;
            minX = MediumMinX; maxX = MediumMaxX;
            minY = MediumMinY; maxY = MediumMaxY;
            minZ = MediumMinZ; maxZ = MediumMaxZ;
        } else if (diff == Difficulty.Hard) {
            holdTime = HardHoldTime;
            minX = HardMinX; maxX = HardMaxX;
            minY = HardMinY; maxY = HardMaxY;
            minZ = HardMinZ; maxZ = HardMaxZ;
        }
        OnMovementUpdate(movement);
    }

    private Vector3 headsetPos;
    private Vector3 rightArmPos;
    private Vector3 leftArmPos;

    private Vector3 currentHeadsetPos;
    private float minimalPos;
    private float maximalPos;

    public override IEnumerator OnMovementUpdate(ExerciseMovement movement)
    {
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

        // Hold at target & accumulate score
        float elapsedWhileHolding = 0f;
        // Debug.Log(ExerciseManager.Instance.Headset.position);
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

            // Checks currently chosen difficulty to see if the exercise needs position check
            Vector3 minBound = new Vector3(minX, minY, minZ);
            Vector3 maxBound = new Vector3(maxX, maxY, maxZ);

            minimalPos = headsetPos.y - minBound.y;
            maximalPos = headsetPos.y - maxBound.y;


            GenericExerciseReferences.Instance.FeedbackLine.SetActive(true);
            GenericExerciseReferences.Instance.RenderLineMinimal.SetActive(true);
            GenericExerciseReferences.Instance.RenderLineMaximal.SetActive(true);

            Vector3 defaultZ = new Vector3(0, 0, 1);

            GenericExerciseReferences.Instance.RenderLineMinimal.transform.position = (headsetPos + defaultZ) - minBound;
            GenericExerciseReferences.Instance.RenderLineMaximal.transform.position = (headsetPos + defaultZ) - maxBound;

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

            GenericExerciseReferences.Instance.HoldImageLine.sizeDelta = new Vector2(Mathf.Lerp(0, 160, elapsedWhileHolding / holdTime), GenericExerciseReferences.Instance.HoldImageLine.sizeDelta.y);

            yield return null;
        }
        while (Input.GetKey(KeyCode.Space))
        { // Need< Or my Schei�e breaks because the next action image will skip aswell
            yield return null;
        }
        yield return null;
    }

    public override void OnMovementEnd(ExerciseMovement movement)
    {
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
        // Debug.Log(headsetPos.y);
        // Debug.Log(currentHeadsetPos.y + " BHEHIFBEHBSEHGBSHGSHEGHSBGHSBEGBSGEJ");
        // Debug.Log(minimalPos);
        // Debug.Log(maximalPos);
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
