using UnityEngine;

[CreateAssetMenu(fileName = "ThresholdBehavior", menuName = "Exercise/Behaviour/Threshold")]
public class ThresholdBehaviorSO : BehaviourSO
{
    public float HoldTime;
    public float MinX;
    public float MaxX;
    public float MinY;
    public float MaxY;
    public float MinZ;
    public float MaxZ;
    public override IMovementBehaviour CreateBehaviour()
    {
        return new ThresholdBehaviour(HoldTime, MinX, MaxY, MinY, MaxY, MinZ, MaxZ);
    }
}
