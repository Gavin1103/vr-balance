using UnityEngine;

[CreateAssetMenu(fileName = "HeightThresholdBehavior", menuName = "Exercise/Behaviour/Height Threshold")]
public class HeightThresholdBehaviorSO : BehaviourSO
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
        return new HeightThresholdBehavior();
    }
}
