using UnityEngine;

[CreateAssetMenu(fileName = "NewCloseEyesMovement", menuName = "Exercise/Movement/Head Sway")]
public class HeadSwayMovementSO : BehaviourSO
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
        return new HeadSwayMovement();
    }
}
