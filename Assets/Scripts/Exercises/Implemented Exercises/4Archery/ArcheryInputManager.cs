using UnityEngine;

public static class ArcheryInputManager
{
    public static event System.Action OnTriggerPulled;
    public static event System.Action OnTriggerReleased;

    public static void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            OnTriggerPulled?.Invoke();
            Debug.Log("helloo");
        }

        if (Input.GetButtonUp("Fire1"))
            OnTriggerReleased?.Invoke();
    }
}
