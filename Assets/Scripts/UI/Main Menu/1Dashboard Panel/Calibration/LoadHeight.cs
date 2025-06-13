using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Globalization;

public class LoadHeight : MonoBehaviour
{
    public List<string> addCalibration = new List<string>();
    public static List<Vector3> loadData = new List<Vector3>();

    public void LoadHeightData()
    {
        string path = Application.persistentDataPath + "/UserHeightList.json";
        string json = File.ReadAllText(path);
        HeightList loadHeight = JsonUtility.FromJson<HeightList>(json);
        addCalibration = loadHeight.heightList;

        loadData.Clear();

        //Debug.Log(addCalibration);

        foreach (string height in addCalibration)
        {
            //Debug.Log("String height: " + height);

            string clean = height.Replace("(", "").Replace(")", ""); // Clean string from parentheses
            string[] split = clean.Split(',').Select(p => p.Trim()).ToArray(); // split the string
            float[] part = split.Select(s => float.Parse(s, CultureInfo.InvariantCulture)).ToArray(); //parse it into float

            Vector3 JsonVector3 = new Vector3(part[0], part[1], part[2]);
            loadData.Add(JsonVector3);
            //Debug.Log(JsonVector3);
        }
    }

    public List<Vector3> LoadData {  get { return loadData; } set { loadData = value; } }
}