using UnityEngine;

[CreateAssetMenu(fileName = "NewCloseEyesMovement", menuName = "Exercise/Movement/Head Sway")]
public class HeadSwayMovementSO : ExerciseMovementSO
{
    public float HoldTime;
    public float MinX;
    public float MaxX;
    public float MinY;
    public float MaxY;
    public float MinZ;
    public float MaxZ;
    public override ExerciseMovement CreateMovement()
    {
        return new HeadSwayMovement(Duration, Image, Score, HoldTime, MinX, MaxX, MinY, MaxY, MinZ, MaxZ);
    }
}
