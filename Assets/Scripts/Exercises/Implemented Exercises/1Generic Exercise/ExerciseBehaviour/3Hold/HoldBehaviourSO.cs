using UnityEngine;

[CreateAssetMenu(fileName = "NewHoldBehaviour", menuName = "Exercise/Behaviour/Hold")]
public class HoldBehaviourSO : BehaviourSO {
    public float HoldTime;

    public override IMovementBehaviour CreateBehaviour() {
        return new HoldBehaviour(HoldTime);
    }
}