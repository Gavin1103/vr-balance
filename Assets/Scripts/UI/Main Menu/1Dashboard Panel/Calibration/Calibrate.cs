using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UIElements;

public class Calibrate : MonoBehaviour
{
    public bool calibrateOn;

    public float calibrateTimer;
    private float calibrateTime;

    public GameObject mainMenu;
    public GameObject calibrationMenu;

    public TextMeshProUGUI timerText;

    public GameObject mainCamera;
    public GameObject rightArm;
    public GameObject leftArm;

    public TMP_Text lengthTextField;
    public TMP_Text leftArmTextField;
    public TMP_Text rightArmTextField;


    void Start()
    {
        calibrateTime = calibrateTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (calibrateOn)
        {
            CalibrateUser();
        }
    }

    public void CalibrateUser()
    {
        calibrateTimer -= Time.deltaTime;
        timerText.text = calibrateTimer.ToString("F2");

        if (calibrateTimer < 0)
        {
            timerText.text = "Done!";
            calibrateOn = false;
            StartCalibration();
            StartCoroutine(Calibrated());
        }
    }

    IEnumerator Calibrated()
    {
        yield return new WaitForSeconds(1f);

        calibrateTimer = calibrateTime;

        mainMenu.SetActive(true);
        calibrationMenu.SetActive(false);
    }

    public void StartCalibration()
    {
        HeadsetCalibration();
        RightArmCalibration();
        LeftArmCalibration();
        
        if (checker == 3)
        {
            addCalibration.Clear();
            SaveLocalCalibration(calibratedHeadsetPosition, calibratedRightArmPosition, calibratedLeftArmPosition);
            CalculateHeadArmDistance(calibratedHeadsetPosition, calibratedRightArmPosition, calibratedLeftArmPosition);
            LoadHeight.LoadHeightData();
            checker = 0;
        }      
    }

    //[SerializeField] private float defaultHeight = 1.8f;
    [SerializeField] private Vector3 calibratedHeadsetPosition;
    [SerializeField] private Vector3 calibratedRightArmPosition;
    [SerializeField] private Vector3 calibratedLeftArmPosition;

    private int checker;
    //[SerializeField] private Vector3 currentheadsetPosition;

    public void HeadsetCalibration()
    {
        Vector3 headHeight = mainCamera.transform.position;
        calibratedHeadsetPosition = headHeight;
        checker++;
    }

    public void RightArmCalibration()
    {
        Vector3 rightArmHeight = rightArm.transform.position;
        calibratedRightArmPosition = rightArmHeight;
        checker++;
    }

    public void LeftArmCalibration()
    {
        Vector3 leftArmHeight = leftArm.transform.position;
        calibratedLeftArmPosition = leftArmHeight;
        checker++;
    }

    public float rightArmLength;
    public float leftArmLength;
    public void CalculateHeadArmDistance(Vector3 headset, Vector3 rightArm, Vector3 leftArm)
    {
        rightArmLength = Vector3.Distance(headset, rightArm);
        leftArmLength = Vector3.Distance(headset, leftArm);

        lengthTextField.text = headset.y.ToString("F2") + "m";
        leftArmTextField.text = leftArmLength.ToString("F2") + "m";
        rightArmTextField.text = rightArmLength.ToString("F2") + "m";
    }

    public List<string> addCalibration = new List<string>();

    public void SaveLocalCalibration(Vector3 headset, Vector3 rightArm, Vector3 leftArm)
    {
        string path = Application.persistentDataPath + "/UserHeightList.json";
        addCalibration.Add(headset.ToString());
        addCalibration.Add(rightArm.ToString());
        addCalibration.Add(leftArm.ToString());

        HeightList list = new HeightList { heightList = addCalibration };
        string newJson = JsonUtility.ToJson(list);
        File.WriteAllText(path, newJson);
    }

    public void TurnOn()
    {
        calibrateOn = true;
    }
}
