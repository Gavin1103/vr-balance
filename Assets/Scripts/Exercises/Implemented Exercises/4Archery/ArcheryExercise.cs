using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcheryExercise : Exercise
{
    private GameObject bowInstance;
    private Transform bowHand;
    private Transform arrowSpawnPoint;

    private GameObject currentArrow;
    private bool isPulling;

    private Coroutine spawnTargetsCoroutine;

    private GameObject arrowPrefab => ArcheryExerciseReferences.Instance.ArrowPrefab;
    private GameObject bowPrefab => ArcheryExerciseReferences.Instance.BowPrefab;
    private GameObject targetPrefab => ArcheryExerciseReferences.Instance.TargetPrefab;
    private Transform bowSpawnPoint => ArcheryExerciseReferences.Instance.BowSpawnPoint;
    private Transform targetArea => ArcheryExerciseReferences.Instance.TargetArea;

    public ArcheryExercise(string title, ExerciseCategory category, string description, List<string> requirements, Sprite image)
        : base(title, category, description, requirements, image)
    {
    }

    public override void StartExercise()
    {
        base.StartExercise();

        bowInstance = GameObject.Instantiate(bowPrefab, bowSpawnPoint.position, bowSpawnPoint.rotation);
        arrowSpawnPoint = bowInstance.transform.Find("ArrowSpawnPoint");

        spawnTargetsCoroutine = ExerciseManager.Instance.StartCoroutine(SpawnTargets());

        ArcheryInputManager.OnTriggerPulled += OnTriggerPulled;
        ArcheryInputManager.OnTriggerReleased += OnTriggerReleased;
    }

    protected override void PlayExercise()
    {
        // Nothing here yet
    }

    private void OnTriggerPulled()
    {
        if (currentArrow == null)
        {
            currentArrow = GameObject.Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
            currentArrow.transform.parent = arrowSpawnPoint;
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
            rb.linearVelocity = arrowSpawnPoint.forward * 25f;
            currentArrow = null;
            isPulling = false;
        }
    }

    private IEnumerator SpawnTargets()
    {
        while (true)
        {
            ArcheryInputManager.Update();
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