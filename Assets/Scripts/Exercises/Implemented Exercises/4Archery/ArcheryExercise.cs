using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DTO.Request.Exercise.@base;

public class ArcheryExercise : Exercise
{
    private float minForce = 0.001f; // When you didnt pull the bow at all
    private float maxForce = 10f; // When you pulled the bow super hard


    // A lot of references and variables that shouldn't really be looked at
    private float spawnInterval;
    private float targetLifetime;
    private List<GameObject> activeTargets = new List<GameObject>();
    private float pullDistance;
    private GameObject bowInstance;
    private Transform bowHand;
    private Transform arrowSpawnPoint;
    private GameObject currentArrow;
    private bool isPulling;
    private Coroutine spawnTargetsCoroutine;
    private BowPullZone pullZone;
    private Transform bowStringVisual;
    private Transform middleRingBone;
    private AffordancePulse pulseEffect;
    private GameObject arrowPrefab => ArcheryExerciseReferences.Instance.ArrowPrefab;
    private GameObject bowPrefab => ArcheryExerciseReferences.Instance.BowPrefab;
    private GameObject targetPrefab => ArcheryExerciseReferences.Instance.TargetPrefab;
    private Transform bowSpawnPoint => ArcheryExerciseReferences.Instance.BowSpawnPoint;
    private Transform targetArea => ArcheryExerciseReferences.Instance.TargetArea;
    private Transform leftHand => ExerciseManager.Instance.LeftStick;
    private Transform rightHand => ExerciseManager.Instance.RightStick;
    private ExerciseService excerciseSerice = new ExerciseService();

    public ArcheryExercise(string title, ExerciseCategory category, string description, List<string> requirements, Sprite image)
        : base(title, category, description, requirements, image)
    {
    }

    public override void StartExercise()
    {
        base.StartExercise();

        switch (DifficultyManager.Instance.SelectedDifficulty)
        {
            case Difficulty.Easy:
                spawnInterval = 3f;
                break;
            case Difficulty.Medium:
                spawnInterval = 2f;
                break;
            case Difficulty.Hard:
                spawnInterval = 1.5f;
                break;
        }
        targetLifetime = spawnInterval * 3f;


        bowInstance = GameObject.Instantiate(bowPrefab, bowSpawnPoint.position, leftHand.rotation);
        bowInstance.transform.SetParent(leftHand);

        arrowSpawnPoint = bowInstance.transform.Find("ArrowSpawnPoint");
        pullZone = bowInstance.transform.Find("PullZone").GetComponent<BowPullZone>();
        bowStringVisual = bowInstance.transform.Find("BowString");
        middleRingBone = bowInstance.transform.Find("Armature/Root/MiddleBone");
        // pulseEffect = bowStringVisual.GetComponentInChildren<AffordancePulse>();

        spawnTargetsCoroutine = ExerciseManager.Instance.StartCoroutine(SpawnTargets());

        ArcheryInputManager.OnTriggerPulled += OnTriggerPulled;
        ArcheryInputManager.OnTriggerReleased += OnTriggerReleased;
    }

    public override void PlayExercise()
    {
        // pulseEffect.enabled = pullZone.handInside;

        if (isPulling && currentArrow != null)
        {
            // Pull vector from bow to right hand
            Vector3 pullDir = rightHand.position - arrowSpawnPoint.position;
            pullDistance = Mathf.Clamp(pullDir.magnitude, 0f, 1f);

            middleRingBone.position = rightHand.position;
        }
        else
        {
            middleRingBone.localPosition = Vector3.zero;
        }
    }

    private void OnTriggerPulled()
    {
        if (currentArrow == null && pullZone.handInside)
        {
            currentArrow = GameObject.Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
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
            if (activeTargets.Count < 3)
            {
                Vector3 randomPos = targetArea.position + new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-0.5f, 1f),
                    Random.Range(-0.5f, 0.5f)
                );

                GameObject target = GameObject.Instantiate(targetPrefab, randomPos, Quaternion.identity);
                activeTargets.Add(target);

                ExerciseManager.Instance.StartCoroutine(DespawnAfterTime(target, targetLifetime));
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private IEnumerator DespawnAfterTime(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (target != null)
        {
            Despawn(target);
        }
    }
    private void Despawn(GameObject target)
    {
        FeedbackManager.Instance.DisplayMissFeedback(target.transform.position);
        activeTargets.Remove(target);
        Object.Destroy(target);
    }

    public override void ExerciseEnded()
    {
        SaveExercise();
        if (bowInstance != null)
            GameObject.Destroy(bowInstance);

        if (spawnTargetsCoroutine != null)
            ExerciseManager.Instance.StopCoroutine(spawnTargetsCoroutine);

        foreach (var target in activeTargets)
        {
            if (target != null)
            {
                Object.Destroy(target);
            }
        }
        activeTargets.Clear();

        ArcheryInputManager.OnTriggerPulled -= OnTriggerPulled;
        ArcheryInputManager.OnTriggerReleased -= OnTriggerReleased;
    }
    
    private void SaveExercise() {
        CompletedExerciseDTO dto = new CompletedExerciseDTO {
            exercise = "Archery",
            earnedPoints = (int)ScoreManager.Instance.Score,
            difficulty = DifficultyManager.Instance.SelectedDifficulty,
            completedAt = System.DateTime.UtcNow
        };

       ExerciseManager.Instance.StartCoroutine(excerciseSerice.SaveExercise(
           dto,
           onSuccess: ApiResponse => {
               Debug.Log(ApiResponse.message);
           },
           onError: error => {
               Debug.Log(error.message);
           },
           "standard" // This should be different to collect unique data
       ));
    }
}