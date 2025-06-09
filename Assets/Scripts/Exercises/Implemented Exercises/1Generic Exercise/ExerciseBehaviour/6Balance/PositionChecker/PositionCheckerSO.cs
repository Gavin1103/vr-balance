using UnityEngine;

public abstract class PositionCheckerSO : ScriptableObject
{
    public float HoldTime;
    public float MinX, MaxX, MinY, MaxY, MinZ, MaxZ;

    public virtual PositionChecker SetPosition()
    {
        return new PositionChecker(HoldTime, MinX, MaxX, MinY, MaxY, MinZ, MaxZ);
    }
}
