using UnityEngine;

public class BowPullZone : MonoBehaviour
{
    public bool handInside = false;
    [HideInInspector] public AffordancePulse pulse;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RightHand"))
        {
            handInside = true;
            pulse.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RightHand"))
        {
            handInside = false;
            pulse.gameObject.SetActive(false);
        }
    }
}
