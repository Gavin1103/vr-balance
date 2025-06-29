using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float despawnTime = 3f;
    private void Start()
    {
        Invoke(nameof(Despawn), despawnTime);
    }

    private void Despawn()
    {
        FeedbackManager.Instance.DisplayMissFeedback(transform.position);
        Destroy(gameObject);
    }
}