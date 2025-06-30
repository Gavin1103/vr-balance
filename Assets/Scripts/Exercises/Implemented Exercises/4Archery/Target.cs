using UnityEngine;

public class Target : MonoBehaviour
{
    private float despawnTime;
    private void Start()
    {
        Invoke(nameof(Despawn), despawnTime);
    }
    public IEnumerator DespawnAfterTime(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (target != null)
        {
            FeedbackManager.Instance.DisplayMissFeedback(target.transform.position);
            activeTargets.Remove(target);
            Destroy(target);
        }
    }
    private void Despawn()
    {
        FeedbackManager.Instance.DisplayMissFeedback(transform.position);
        Destroy(gameObject);
    }
}