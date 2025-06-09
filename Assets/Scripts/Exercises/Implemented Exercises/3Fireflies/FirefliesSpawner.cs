using System.Collections.Generic;
using System;
using UnityEngine;

public class FirefliesSpawner : MonoBehaviour {
    [SerializeField] private List<GameObject> fireFly;         // List of available FireFly prefabs to spawn
    [SerializeField] private List<Transform> spawnPoints;      // List of spawn points in the scene
    [SerializeField] private float gizmoRadius = 1f;           // Radius for visualizing spawn areas

    private List<GameObject> spawnedFireflies = new(); // bijhouden wat gespawned is

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

    public void SpawnFireFly(int amount, int type) {
        for (int i = 0; i < amount; i++) {

            // Pick a random spawn point
            int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomIndex];

            // Instantiate the firefly at the selected spawn point
            GameObject instance = Instantiate(fireFly[type], spawnPoint.position, Quaternion.identity);
            spawnedFireflies.Add(instance);
        }
    }
    public void SpawnRandomFireFly(int amount) {
        for (int i = 0; i < amount; i++) {

            // Pick a random prefab and a random spawn point
            int randomFireFlyIndex = UnityEngine.Random.Range(0, fireFly.Count);
            int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomIndex];

            // Instantiate the firefly at the selected spawn point
            GameObject instance = Instantiate(fireFly[randomFireFlyIndex], spawnPoint.position, Quaternion.identity);
            spawnedFireflies.Add(instance);
        }
    }
    public void ClearAllFireflies() {
        foreach (var f in spawnedFireflies) {
            if (f != null)
                Destroy(f);
        }

        spawnedFireflies.Clear();
    }
    /// <summary>
    /// Draws gizmos in the editor to visualize each spawn point.
    /// </summary>
    private void OnDrawGizmos() {
        if (spawnPoints == null) return;

        Gizmos.color = Color.yellow;

        foreach (Transform t in spawnPoints) {
            if (t != null) {
                Gizmos.DrawWireSphere(t.position, gizmoRadius);
            }
        }
    }
}
