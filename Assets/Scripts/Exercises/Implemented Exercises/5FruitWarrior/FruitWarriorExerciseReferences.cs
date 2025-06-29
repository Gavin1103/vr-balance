using UnityEngine;

public class FruitWarriorExerciseReferences : MonoBehaviour
{
    public static FruitWarriorExerciseReferences Instance;

    public GameObject[] Fruit;
    public Transform FruitSpawnPoint;
    public GameObject Sword;
    public Transform LeftSwordSpawnPoint;
    public Transform RightSwordSpawnPoint;

    void Awake()
    {
        Instance = this;
    }
}