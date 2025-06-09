using UnityEngine;

[CreateAssetMenu(fileName = "NewHoldBehaviour", menuName = "Exercise/Behaviour/OneShot")]
public class OneShotBehaviourSO : BehaviourSO {
    public float Score;

    public override IMovementBehaviour CreateBehaviour() {
        return new OneShotBehaviour(Score);
    }
}