using UnityEngine;

[CreateAssetMenu(fileName = "ThresholdBehaviour", menuName = "Exercise/Behaviour/Threshold")]
public class ThresholdBehaviourSO : BehaviourSO
{
    public float HoldTime;
    public float MinX;
    public float MaxX;
    public float MinY;
    public float MaxY;
    public float MinZ;
    public float MaxZ;

    [Header("Easy")]
    public float EasyHoldTime;

    public float EasyMinX, EasyMaxX, EasyMinY, EasyMaxY, EasyMinZ, EasyMaxZ;


    [Header("Medium")]
    public float MediumHoldTime;
    public float MediumMinX, MediumMaxX, MediumMinY, MediumMaxY, MediumMinZ, MediumMaxZ;


    [Header("Hard")]
    public float HardHoldTime;
    public float HardMinX, HardMaxX, HardMinY, HardMaxY, HardMinZ, HardMaxZ;

    public override IMovementBehaviour CreateBehaviour() {
        ThresholdBehaviour b = new ThresholdBehaviour();
        b.SetDifficultyVariables(
            EasyHoldTime, EasyMinX, EasyMaxX, EasyMinY, EasyMaxY, EasyMinZ, EasyMaxZ,
            MediumHoldTime, MediumMinX, MediumMaxX, MediumMinY, MediumMaxY, MediumMinZ, MediumMaxZ,
            HardHoldTime, HardMinX, HardMaxX, HardMinY, HardMaxY, HardMinZ, HardMaxZ
        );
        return b;
    }
}
