using UnityEngine;
using UnityEngine.InputSystem;

public class ArcheryInputManager : MonoBehaviour
{
    public static ArcheryInputManager Instance { get; private set; }

    public static event System.Action OnTriggerPulled;
    public static event System.Action OnTriggerReleased;

    private ArcheryInputActions inputActions;

    private void Awake()
    {
        Instance = this;
        inputActions = new ArcheryInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Gameplay.Fire.started += ctx => OnTriggerPulled?.Invoke();
        inputActions.Gameplay.Fire.canceled += ctx => OnTriggerReleased?.Invoke();
    }

    private void OnDisable()
    {
        inputActions.Gameplay.Fire.started -= ctx => OnTriggerPulled?.Invoke();
        inputActions.Gameplay.Fire.canceled -= ctx => OnTriggerReleased?.Invoke();

        inputActions.Disable();
    }
}