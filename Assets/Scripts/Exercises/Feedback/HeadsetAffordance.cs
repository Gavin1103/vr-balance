using UnityEngine;

public class HeadsetAffordance : MonoBehaviour {
    public float maxDistance = 1.0f; // Distance at which opacity is fully visible
    public float minDistance = 0.1f; // Distance at which opacity is lowest
    public float minOpacity = 0.2f;  // Minimum alpha value

    private Material mat;

    void Awake() {
        // Get the material (assumes the sphere uses a Renderer with a single material)
        mat = GetComponent<Renderer>().material;
    }

    void Update() {
        if (ExerciseManager.Instance == null || ExerciseManager.Instance.Headset == null) return;

        float distance = Vector3.Distance(transform.position, ExerciseManager.Instance.Headset.position);

        // Map distance to opacity: closer = lower opacity
        float t = Mathf.InverseLerp(maxDistance, minDistance, distance);
        float alpha = Mathf.Lerp(1f, minOpacity, t);

        Color color = mat.color;
        color.a = alpha;
        mat.color = color;
    }
}