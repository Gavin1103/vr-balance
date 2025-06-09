using UnityEngine;

[CreateAssetMenu(fileName = "NewMustBehaviour", menuName = "Exercise/Behaviour/Must")]
public class MustBehaviourSO : BehaviourSO {
    public override IMovementBehaviour CreateBehaviour() {
        return new MustBehaviour();
    }
}