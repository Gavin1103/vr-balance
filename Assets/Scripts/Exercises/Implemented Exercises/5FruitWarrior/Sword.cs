using UnityEngine;

public class Sword : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fruit"))
        {
            ScoreManager.Instance.AddScore(35);
            FeedbackManager.Instance.DisplayFeedback(FeedbackType.Great, other.gameObject.transform.position);
            Destroy(other.gameObject);
        }
    }
}