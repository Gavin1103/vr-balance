using UnityEngine;

public class CalibrationButton : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject calibrationMenu;
    public Calibrate calibrate;
    void OnEnable()
    {
        LoadHeight.LoadHeightData();

        if (Calibrate.HasCalibrated)
        {
            Vector3 headset = LoadHeight.LoadData[0];
            Vector3 rightArm = LoadHeight.LoadData[1];
            Vector3 leftArm = LoadHeight.LoadData[2];

            calibrate.CalculateHeadArmDistance(headset, rightArm, leftArm);
        }
        else
        {
            calibrate.UnknownLengths();
        }
    }

    public void OpenCalibrationMenu()
    {
        mainMenu.SetActive(false);
        calibrationMenu.SetActive(true);
    }
}
