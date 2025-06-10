using UnityEngine;

public class RandomFlightWithinRadius : MonoBehaviour {
    [SerializeField] private float radius = 5f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private Transform centerTransform; // Editor-preview voor Gizmos

    private Vector3 centerPoint;
    private Vector3 targetPosition;

    private void Start() {
        // Gebruik ofwel een meegegeven transform, of gewoon eigen positie
        centerPoint = centerTransform != null ? centerTransform.position : transform.position;

        PickNewTargetPosition();
    }

    private void Update() {
        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        float angleToTarget = Vector3.Angle(transform.forward, direction);
        if (angleToTarget < 10f) {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

        if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance) {
            PickNewTargetPosition();
        }
    }

    public void SetCenterPoint(Transform newCenter) {
        centerTransform = newCenter;
        centerPoint = newCenter.position;
    }

    private void PickNewTargetPosition() {
        Vector3 randomOffset = Random.insideUnitSphere * radius;
        targetPosition = centerPoint + randomOffset;
    }

    private void OnDrawGizmosSelected() {
        if (centerTransform == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centerTransform.position, radius);
    }
}

