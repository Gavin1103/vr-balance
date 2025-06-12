using UnityEngine;

public class CalibrationButton : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject calibrationMenu;

    public void OpenCalibrationMenu()
    {
        mainMenu.SetActive(false);
        calibrationMenu.SetActive(true);
    }
}
