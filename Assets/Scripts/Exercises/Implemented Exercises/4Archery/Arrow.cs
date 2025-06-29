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

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Target"))
        {
            ScoreManager.Instance.AddScore(10);
            Destroy(col.gameObject);
        }

        Destroy(gameObject, 1f);
    }
}