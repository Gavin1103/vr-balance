using UnityEngine;

[CreateAssetMenu(fileName = "NewMustBehaviour", menuName = "Exercise/Behaviour/Must")]
public class MustBehaviourSO : BehaviourSO {
    public float HoldTime = 3f;

    public override IMovementBehaviour CreateBehaviour()
    {
        return new MustBehaviour(HoldTime);
    }
}