using System.Collections.Generic;
using UnityEngine;

public class ButterFlySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> butterFly;         // List of available FireFly prefabs to spawn
    [SerializeField] private List<Transform> spawnPoints;      // List of spawn points in the scene
    [SerializeField] private float gizmoRadius = 1f;           // Radius for visualizing spawn areas

    private List<GameObject> spawnedButterflies = new(); // bijhouden wat gespawned is

    private void Awake() {
        // Ensure that spawn points are assigned, otherwise log an error
        if (spawnPoints == null || spawnPoints.Count == 0) {
            Debug.LogError("SpawnPoints is null ", this);
        }
    }

    /// <summary>
    /// Spawns a given number of fireflies at random spawn points with random prefabs.
    /// </summary>
    /// <param name="amount">The number of fireflies to spawn</param>

    public void SpawnButterfly(int amount, int type) {
        for (int i = 0; i < amount; i++) {

            // Pick a random spawn point
            int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomIndex];

            // Instantiate the firefly at the selected spawn point
            GameObject instance = Instantiate(butterFly[type], spawnPoint.position, Quaternion.identity);
            spawnedButterflies.Add(instance);
        }
    }
}
