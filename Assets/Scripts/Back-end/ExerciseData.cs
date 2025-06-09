using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;
using System.IO;
using System;

[System.Serializable]
public class CompletedExerciseData {
    public int userId;
    public Exercise exercise;
    public string difficulty;
    public int earnedPoints;
    public DateTime startTime;
    public DateTime endTime = DateTime.Now;

    public List<PositionRotationTimestamp> headsetData = new List<PositionRotationTimestamp>();
    public List<PositionRotationTimestamp> leftControllerData = new List<PositionRotationTimestamp>();
    public List<PositionRotationTimestamp> rightControllerData = new List<PositionRotationTimestamp>();
}

[System.Serializable]
public class PositionRotationTimestamp {
    public Vector3 position;
    public Quaternion rotation;
    public float timestamp;

    public PositionRotationTimestamp(Vector3 position, Quaternion rotation, float timestamp) {
        this.position = position;
        this.rotation = rotation;
        this.timestamp = timestamp;
    }
}