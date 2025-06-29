using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FruitWarriorExercise : Exercise
{
    private GameObject leftSword;
    private GameObject rightSword;

    private Coroutine spawnFruitsCoroutine;
    public FruitWarriorExercise(string title, ExerciseCategory category, string description, List<string> requirements, Sprite image)
        : base(title, category, description, requirements, image)
    {
    }

    public override void StartExercise()
    {
        base.StartExercise();
        
        leftSword = GameObject.Instantiate(FruitWarriorExerciseReferences.Instance.Sword, FruitWarriorExerciseReferences.Instance.LeftSwordSpawnPoint.position, ExerciseManager.Instance.LeftStick.rotation);
        rightSword = GameObject.Instantiate(FruitWarriorExerciseReferences.Instance.Sword, FruitWarriorExerciseReferences.Instance.RightSwordSpawnPoint.position, ExerciseManager.Instance.RightStick.rotation);
        leftSword.transform.SetParent(ExerciseManager.Instance.LeftStick);
        rightSword.transform.SetParent(ExerciseManager.Instance.RightStick);

        spawnFruitsCoroutine = ExerciseManager.Instance.StartCoroutine(SpawnFruits());
    }

    public override void PlayExercise() {

    }


    private IEnumerator SpawnFruits()
    {
        var refData = FruitWarriorExerciseReferences.Instance;

        while (true)
        {
            // Pick random fruit
            GameObject fruitPrefab = refData.Fruit[Random.Range(0, refData.Fruit.Length)];

            Vector3 spawnPos = refData.FruitSpawnPoint.position;
            spawnPos.y = 0f;
            spawnPos.x += Random.Range(-1f, 1f);
            spawnPos.z += Random.Range(-1f, 1f);

            GameObject fruit = GameObject.Instantiate(fruitPrefab, spawnPos, Random.rotation);

            yield return new WaitForSeconds(Random.Range(0.2f, 0.7f));
        }
    }


    public override void ExerciseEnded()
    {
        GameObject.Destroy(leftSword);
        GameObject.Destroy(rightSword);

        if (spawnFruitsCoroutine != null)
            ExerciseManager.Instance.StopCoroutine(spawnFruitsCoroutine);
    }
}