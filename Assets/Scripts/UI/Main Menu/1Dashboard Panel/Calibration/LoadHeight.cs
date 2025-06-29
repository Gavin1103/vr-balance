using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Globalization;

public static class LoadHeight
{
    public static List<string> AddCalibration = new List<string>();
    public static List<Vector3> LoadData = new List<Vector3>();

    public static void LoadHeightData()
    {
        string path = Application.persistentDataPath + "/UserHeightList.json";

        if (!File.Exists(path))
        {
            // File doesn't exist, assume default height
            LoadData.Clear();
            Vector3 defaultHeight = new Vector3(0, 1.7f, 0);
            LoadData.Add(defaultHeight);
            Debug.LogWarning("UserHeightList.json not found. Assuming default height of 1.7m...");
            return;
        }

        string json = File.ReadAllText(path);
        HeightList loadHeight = JsonUtility.FromJson<HeightList>(json);
        AddCalibration = loadHeight.heightList;

        LoadData.Clear();

        if (AddCalibration == null || AddCalibration.Count == 0)
        {
            Vector3 defaultHeight = new Vector3(0, 1.7f, 0);
            LoadData.Add(defaultHeight);
            Debug.LogWarning("The user didn't calibrate their height. Assuming default height of 1.7m...");
            return;
        }

        foreach (string height in AddCalibration)
        {
            string clean = height.Replace("(", "").Replace(")", "");
            string[] split = clean.Split(',').Select(p => p.Trim()).ToArray();
            float[] part = split.Select(s => float.Parse(s, CultureInfo.InvariantCulture)).ToArray();

            Vector3 JsonVector3 = new Vector3(part[0], part[1], part[2]);
            LoadData.Add(JsonVector3);
        }
    }
}