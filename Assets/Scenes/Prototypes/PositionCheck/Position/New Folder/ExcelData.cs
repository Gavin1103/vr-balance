using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Unity.XR.GoogleVr;
using UnityEditor;
using UnityEngine;

public class ExcelData : MonoBehaviour
{


    PositionManager positionManager;
    public GameObject excelData;

    public Vector3 testPosition;

    bool collectData = false;

    int iterations = 0;

    string setData;
    string extendedPath = "/Scenes/Prototypes/Experiment/";
    string dayPath;
    string exerciseDate;

    string folderDir = "Assets/Scenes/Prototypes/Experiment";
    DateTime dateTime = DateTime.Now;
    void Start()
    {
        Debug.Log("Test");
        dayPath = dateTime.ToString("dddd-dd");
        positionManager = excelData.GetComponent<PositionManager>();
        if (!Directory.Exists(Application.dataPath + extendedPath + dayPath))
        {
            Debug.Log("Test");
            //AssetDatabase.CreateFolder(folderDir, dayPath);
        }
    }

    public string currentExercise;
    public void StartDataCollection(string name)
    {
        currentExercise = name;
        collectData = true;
  
        exerciseDate = dateTime.ToString("_dddd-H-mm-ss");

        setData = Application.dataPath + extendedPath + dayPath + "/" + currentExercise + exerciseDate + ".csv";     
    }

    public void EndDataCollection()
    {
        collectData = false;
        WriteCSV();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (collectData) {
            testPosition.x = positionManager.CurrentHeadsetPosition.x;
            testPosition.y = positionManager.CurrentHeadsetPosition.y;
            testPosition.z = positionManager.CurrentHeadsetPosition.z;
            iterations++;
            DataPush(iterations, testPosition.x, testPosition.y, testPosition.z);
        }
    }

    List<int> index = new List<int>();
    List<float> xPos = new List<float>();
    List<float> yPos = new List<float>();
    List<float> zPos = new List<float>();
    public void DataPush(int ID, float X, float Y, float Z)
    {
        index.Add(ID);
        xPos.Add(X);
        yPos.Add(Y);
        zPos.Add(Z);
    }

    public void WriteCSV()
    {
        TextWriter tw = new StreamWriter(setData, false);
        tw.WriteLine("ID; X; Y; Z");
        tw.Close();

        tw = new StreamWriter(setData, true);

        for (int i = 0; i < iterations; i++)
        {
            tw.WriteLine(index[i] + ";" + xPos[i] + ";" + yPos[i] + ";" + zPos[i]);
        }
        tw.Close();
    }
}
