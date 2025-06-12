using System;
using System.Collections.Generic;
using UnityEngine;

public class FireFly : MonoBehaviour, Icatchable {

    // Event triggered when the firefly is successfully caught
    public static event Action OnCaught;

    private Vector3 startpos;
    [SerializeField] private float movementSpeed = 0.2f;             // Speed of horizontal noise movement
    [SerializeField] private float verticalBounceSpeed = 2f;         // Speed of up-down movement
    [SerializeField] private float verticalBounceHeight = 0.5f;      // Height of vertical bouncing
    [SerializeField] private float horizontalRange = 1.5f;           // Range of horizontal movement

    [SerializeField] private float motionIntensityVariationSpeed = 0.5f; // How fast intensity changes
    [SerializeField] private float motionIntensityMin = 0.3f;        // Minimum movement intensity
    [SerializeField] private float motionIntensityMax = 1f;          // Maximum movement intensity
    [SerializeField] private float radius = 2.0f;                    // Maximum distance from start position

    private float randomSeed;

    [SerializeField] private List<FireFlyNet.NetType> allowedNetTypes; // Nets that can catch this FireFly

    private void Awake() {
        // Store the starting position and generate a unique seed for this instance
        startpos = transform.position;
        randomSeed = UnityEngine.Random.value * 1000f;
    }
    private void Update() {
        //FlyAroundPoint();
    }

    /// <summary>
    /// Simulates natural floating movement using Perlin noise and sine waves.
    /// </summary>
    private void FlyAroundPoint() {
        float t = Time.time * movementSpeed;

        // Get a smooth varying intensity based on noise
        float intensityNoise = Mathf.PerlinNoise(Time.time * motionIntensityVariationSpeed + randomSeed, 0f);
        float movementIntensity = Mathf.Lerp(motionIntensityMin, motionIntensityMax, intensityNoise);

        // Horizontal movement using Perlin noise
        float x = Mathf.PerlinNoise(t + randomSeed, 0) * 2 - 1;
        float z = Mathf.PerlinNoise(0, t + randomSeed) * 2 - 1;

        // Vertical movement using sine wave
        float y = Mathf.Sin(Time.time * verticalBounceSpeed + randomSeed) * verticalBounceHeight;
        Vector3 randomOffset = new Vector3(x * horizontalRange, y, z * horizontalRange);

        // Apply intensity variation
        randomOffset *= movementIntensity;

        // Clamp total offset to stay within defined radius
        randomOffset = Vector3.ClampMagnitude(randomOffset, radius);

        // Update position
        transform.position = startpos + randomOffset;
    }


    /// <summary>
    /// Called when the FireFly is caught by a net. Destroys the object if allowed.
    /// </summary>
    /// <param name="netType">The type of net used to catch</param>
    public void Catch(FireFlyNet.NetType netType) {
        if (allowedNetTypes.Contains(netType)) {
            OnCaught?.Invoke(); // Trigger the event for other systems (like WaveManager)
            SoundManager.soundInstance.PlaySFX("FireFlyCatch");
            //ScoreManager.ScoreInstance.AddScore(1);
            Destroy(gameObject);
        }
        else {
            Debug.Log("Wrong net type, can't catch this fly.");
        }
    }
}


