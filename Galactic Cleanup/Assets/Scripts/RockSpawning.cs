using UnityEngine;

public class RockSpawning : MonoBehaviour
{
    public GameObject[] asteroidPrefabs; // Array of prefabs
    public Transform playerTransform;
    public float spawnDistance = 50f; // Initial spawn distance
    public float initialSpawnRate = 2f; // Initial spawn rate
    public float spawnRateIncreaseInterval = 30f; // Interval for increasing spawn rate
    public float spawnRateIncreaseAmount = 0.5f; // Increase in spawn rate
    public Vector3 spawnAreaSize = new Vector3(10f, 10f, 10f);
    public int numberOfAsteroids = 5;
    public float maxScale = 1f;
    public float destroyDistance = 60f; // Distance beyond which asteroids will be destroyed

    private float nextSpawnTime = 0f;
    private float elapsedTime = 0f;
    private float currentSpawnRate;

    void Start()
    {
        currentSpawnRate = initialSpawnRate;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Check if it's time to spawn a new group of asteroids.
        if (Time.time > nextSpawnTime)
        {
            SpawnAsteroidGroup();
            nextSpawnTime = Time.time + 1f / currentSpawnRate;
        }

        // Increase spawn rate every 30 seconds
        if (elapsedTime >= spawnRateIncreaseInterval)
        {
            currentSpawnRate += spawnRateIncreaseAmount; // Increase spawn rate
            elapsedTime = 0f; // Reset elapsed time
        }

        // Scale and destroy asteroids
        ScaleAndDestroyAsteroids();
    }

    void SpawnAsteroidGroup()
    {
        // Loop through and spawn multiple asteroids in a single batch.
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            SpawnAsteroid();
        }
    }

    void SpawnAsteroid()
    {
        // Calculate the base spawn position in front of the player based on the specified spawn distance.
        Vector3 spawnPosition = playerTransform.position + playerTransform.forward * spawnDistance;
        // Add randomness to the X, Y, and Z coordinates to spread out the asteroids in the spawn area.
        spawnPosition += new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        // Select a random prefab from the array
        GameObject selectedPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

        // Create the asteroid prefab at the calculated spawn position.
        GameObject asteroid = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        // Start the asteroid small
        asteroid.transform.localScale = Vector3.zero;
    }

    void ScaleAndDestroyAsteroids()
    {
        // Find all asteroids in the scene
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject asteroid in asteroids)
        {
            float distance = Vector3.Distance(asteroid.transform.position, playerTransform.position);
            float scale = Mathf.Lerp(0, maxScale, Mathf.Pow(1 - (distance / destroyDistance), 2)); 
            asteroid.transform.localScale = new Vector3(scale, scale, scale);

            // Destroy the asteroid if it moves too far away
            if (distance > destroyDistance)
            {
                Destroy(asteroid);
            }
        }
    }
}

