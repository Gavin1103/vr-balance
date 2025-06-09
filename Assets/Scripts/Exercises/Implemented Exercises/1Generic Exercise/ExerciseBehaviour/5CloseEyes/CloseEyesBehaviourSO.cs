using UnityEngine;

[CreateAssetMenu(fileName = "NewCloseEyesBehaviour", menuName = "Exercise/Behaviour/Close Eyes")]
public class CloseEyesBehaviourSO : BehaviourSO {
    public override IMovementBehaviour CreateBehaviour() {
        return new CloseEyesBehaviour();
    }
}