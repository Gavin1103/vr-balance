using UnityEngine;

public class Arrow : MonoBehaviour
{
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