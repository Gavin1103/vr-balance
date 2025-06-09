using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu(fileName = "PosHardSO", menuName = "PositionChecker/PosHardSO")]
public class PosHardSO : PositionCheckerSO
{
    public override PositionChecker SetPosition()
    {
        return new PositionCheckerHard(HoldTime, MinX, MaxX, MinY, MaxY, MinZ, MaxZ);
    }
}

