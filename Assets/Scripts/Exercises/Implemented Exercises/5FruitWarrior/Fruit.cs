using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField] private float despawnTime = 3f;

    [SerializeField] private float upwardForce = 60f;
    [SerializeField] private float torqueForce = 5f;
    private Rigidbody rb;

    void Start()
    {
        SoundManager.Instance.PlaySFX("FireFlyCatch");

        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
        rb.AddTorque(Random.onUnitSphere * torqueForce, ForceMode.Impulse);

        Invoke(nameof(Despawn), despawnTime);
    }
    private void Despawn()
    {
        FeedbackManager.Instance.DisplayMissFeedback(transform.position);
        Destroy(gameObject);
    }
}