using UnityEngine;

[CreateAssetMenu(fileName = "NewBalanceBehaviour", menuName = "Exercise/Generic/Behaviour/Balance")]
public class BalanceBehaviourSO : BehaviourSO {
    public float HoldTime;

    public override IMovementBehaviour CreateBehaviour() {
        return new BalanceBehaviour(HoldTime);
    }
}