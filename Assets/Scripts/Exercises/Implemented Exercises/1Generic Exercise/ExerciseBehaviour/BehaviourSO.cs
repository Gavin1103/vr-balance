using UnityEngine;

public abstract class BehaviourSO : ScriptableObject {
    public abstract IMovementBehaviour CreateBehaviour();
}