using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu(fileName = "PosMediumSO", menuName = "PositionChecker/PosMediumSO")]
public class PosMediumSO : PositionCheckerSO
{
    public override PositionChecker SetPosition()
    {
        return new PositionCheckerMedium(HoldTime, MinX, MaxX, MinY, MaxY, MinZ, MaxZ);
    }
}
