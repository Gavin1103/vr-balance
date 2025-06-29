using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcheryExercise : Exercise
{
    private float minForce = 10f;
    private float maxForce = 50f;

    private GameObject bowInstance;
    private Transform bowHand;
    private Transform arrowSpawnPoint;

    private GameObject currentArrow;
    private bool isPulling;

    private Coroutine spawnTargetsCoroutine;
    private BowPullZone pullZone;
    private Transform bowStringVisual;

    private GameObject arrowPrefab => ArcheryExerciseReferences.Instance.ArrowPrefab;
    private GameObject bowPrefab => ArcheryExerciseReferences.Instance.BowPrefab;
    private GameObject targetPrefab => ArcheryExerciseReferences.Instance.TargetPrefab;
    private Transform bowSpawnPoint => ArcheryExerciseReferences.Instance.BowSpawnPoint;
    private Transform targetArea => ArcheryExerciseReferences.Instance.TargetArea;

    private Transform leftHand => ExerciseManager.Instance.LeftStick;
    private Transform rightHand => ExerciseManager.Instance.RightStick;
    public ArcheryExercise(string title, ExerciseCategory category, string description, List<string> requirements, Sprite image)
        : base(title, category, description, requirements, image)
    {
    }

    public override void StartExercise()
    {
        base.StartExercise();
        
        bowInstance = GameObject.Instantiate(bowPrefab, bowSpawnPoint.position, leftHand.rotation);
        bowInstance.transform.SetParent(leftHand);

        arrowSpawnPoint = bowInstance.transform.Find("ArrowSpawnPoint");
        pullZone = bowInstance.transform.Find("PullZone").GetComponent<BowPullZone>();
        bowStringVisual = bowInstance.transform.Find("BowString");


        spawnTargetsCoroutine = ExerciseManager.Instance.StartCoroutine(SpawnTargets());

        ArcheryInputManager.OnTriggerPulled += OnTriggerPulled;
        ArcheryInputManager.OnTriggerReleased += OnTriggerReleased;
    }

    public override void PlayExercise() {
        bowStringVisual.GetComponentInChildren<AffordancePulse>().enabled = pullZone.handInside;

        if (isPulling && currentArrow != null)
        {
            // Pull vector from bow to right hand
            Vector3 pullDir = rightHand.position - arrowSpawnPoint.position;
            float pullDistance = Mathf.Clamp(pullDir.magnitude, 0f, 1f);

            // Visually move string
            bowStringVisual.position = Vector3.Lerp(arrowSpawnPoint.position, rightHand.position, 0.5f);

            // scale pulse effect or color change
        }
    }

    private void OnTriggerPulled()
    {
        if (currentArrow == null && pullZone.handInside)
        {
            currentArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
            currentArrow.transform.parent = arrowSpawnPoint;

            Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;

            isPulling = true;
        }
    }

    private void OnTriggerReleased()
    {
        if (isPulling && currentArrow != null)
        {
            currentArrow.transform.parent = null;

            Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;

            float power = Mathf.Lerp(minForce, maxForce, pullDistance);
            rb.linearVelocity = arrowSpawnPoint.forward * power;

            currentArrow = null;
            isPulling = false;
            pullDistance = 0f;

            // Reset string visual
            bowStringVisual.localPosition = Vector3.zero;
        }
    }

    private IEnumerator SpawnTargets()
    {
        while (true)
        {
            Vector3 randomPos = targetArea.position + new Vector3(
                Random.Range(-1f, 1f), 
                Random.Range(-0.5f, 1f), 
                Random.Range(-0.5f, 0.5f)
            );

            GameObject target = GameObject.Instantiate(targetPrefab, randomPos, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
    }

    public override void ExerciseEnded()
    {
        if (bowInstance != null)
            GameObject.Destroy(bowInstance);

        if (spawnTargetsCoroutine != null)
            ExerciseManager.Instance.StopCoroutine(spawnTargetsCoroutine);

        ArcheryInputManager.OnTriggerPulled -= OnTriggerPulled;
        ArcheryInputManager.OnTriggerReleased -= OnTriggerReleased;
    }
}