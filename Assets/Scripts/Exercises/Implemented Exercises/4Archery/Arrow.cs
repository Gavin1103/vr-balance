using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float despawnTime = 3f;
    private void Start()
    {
        Invoke(nameof(Despawn), despawnTime);
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            ScoreManager.Instance.AddScore(200);
            FeedbackManager.Instance.DisplayFeedback(FeedbackType.Great, other.gameObject.transform.position);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}