using UnityEngine;

public class BowPullZone : MonoBehaviour
{
    public bool handInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RightHand"))
            handInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RightHand"))
            handInside = false;
    }
}
