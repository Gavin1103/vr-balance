using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu(fileName = "PosEasySO", menuName = "PositionChecker/PosEasySO")]
public class PosEasySO : PositionCheckerSO
{
    public override PositionChecker SetPosition()
    {
        return new PositionCheckerEasy(HoldTime, MinX, MaxX, MinY, MaxY, MinZ, MaxZ);
    }
}
