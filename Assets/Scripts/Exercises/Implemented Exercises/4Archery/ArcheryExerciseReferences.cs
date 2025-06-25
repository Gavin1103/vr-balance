using UnityEngine;

public class ArcheryExerciseReferences : MonoBehaviour
{
    public static ArcheryExerciseReferences Instance;

    public GameObject BowPrefab;
    public GameObject ArrowPrefab;
    public GameObject TargetPrefab;

    public Transform BowSpawnPoint;
    public Transform TargetArea;

    void Awake()
    {
        Instance = this;
    }
}