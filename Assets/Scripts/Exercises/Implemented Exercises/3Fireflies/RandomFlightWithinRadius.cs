using UnityEngine;

public class RandomFlightWithinRadius : MonoBehaviour {
    [SerializeField] private float radius = 5f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f; // hoe snel hij draait
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private Transform centerPoint;

    private Vector3 targetPosition;

    private void Start() {
        if (centerPoint == null)
            centerPoint = transform;

        PickNewTargetPosition();
    }

    private void Update() {
        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Beweeg pas als hij bijna goed gericht is (kleine hoek-afwijking)
        float angleToTarget = Vector3.Angle(transform.forward, direction);
        if (angleToTarget < 10f) {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

        // Nieuwe target als dichtbij
        if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance) {
            PickNewTargetPosition();
        }
    }


    private void PickNewTargetPosition() {
        Vector3 randomOffset = Random.insideUnitSphere * radius;
        targetPosition = centerPoint.position + randomOffset;
    }

    private void OnDrawGizmosSelected() {
        if (centerPoint == null)
            centerPoint = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centerPoint.position, radius);
    }
}
