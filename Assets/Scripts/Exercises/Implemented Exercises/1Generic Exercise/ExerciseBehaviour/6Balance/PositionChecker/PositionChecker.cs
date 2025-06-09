using System.Collections;
using UnityEngine;

public class PositionChecker
{
    public float HoldTime;
    public float MinX, MaxX, MinY, MaxY, MinZ, MaxZ;

    public PositionChecker(float holdTime, float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
    {
        HoldTime = holdTime;
        MinX = minX;
        MaxX = maxX;
        MinY = minY;
        MaxY = maxY;
        MinZ = minZ;
        MaxZ = maxZ;
    }
}
