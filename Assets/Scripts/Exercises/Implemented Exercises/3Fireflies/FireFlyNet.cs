using UnityEngine;

public class FireFlyNet : MonoBehaviour {

    // Enum to define the type of net (e.g., right hand or left hand)
    public enum NetType { RightNet, LeftNet }

    [SerializeField] private NetType selectedNet; // Assigned net type for this net object

    /// <summary>
    /// Triggered when another collider enters the trigger zone of this net.
    /// Checks if the object implements Icatchable and attempts to catch it.
    /// </summary>
    /// <param name="other">The collider that entered the trigger</param>
    private void OnTriggerEnter(Collider other) {

        // Try to get a reference to an object that implements Icatchable
        Icatchable catchableObject = other.GetComponent<Icatchable>();

        // If it implements the interface, call the Catch method with this net's type
        if (catchableObject != null) {
            catchableObject.Catch(selectedNet);
            Debug.Log("Caught an object with ICatchable interface!");
        }
    }
}
