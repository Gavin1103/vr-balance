using UnityEngine;

[CreateAssetMenu(fileName = "NewAffordanceBehaviour", menuName = "Exercise/Generic/Behaviour/Affordance")]
public class AffordanceBehaviourSO : BehaviourSO {

    public override IMovementBehaviour CreateBehaviour() {
        return new AffordanceBehaviour();
    }
}