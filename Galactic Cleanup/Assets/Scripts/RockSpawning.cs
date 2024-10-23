using UnityEngine;

public class RockSpawning : MonoBehaviour
{
    // This is the asteroid prefab that will be randomly spawned and is assigned in the inspector.
    public GameObject asteroidPrefab;

    // Referencing the player's transform to track their position in the scene.
    public Transform playerTransform;

    // This controls how far ahead of the player the asteroids will spawn.
    public float spawnDistance = 50f;

    // This controls how frequently asteroids will spawn.
    public float spawnRate = 2f;

    // This defines the size of the area where the asteroids can spawn.
    public Vector3 spawnAreaSize = new Vector3(10f, 10f, 10f);

    // Number of asteroids to spawn at once
    public int numberOfAsteroids = 5;

    // This keeps track of when the next asteroid should spawn.
    private float nextSpawnTime = 0f;

    void Update()
    {
        // Check if it's time to spawn a new group of asteroids.
        if (Time.time > nextSpawnTime)
        {
            // Call the method to spawn a group of asteroids.
            SpawnAsteroidGroup();

            // Set the next spawn time by adding the spawn rate.
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
    }

    // This function handles spawning a group of asteroids at random positions.
    void SpawnAsteroidGroup()
    {
        // Loop through and spawn multiple asteroids in a single batch.
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            SpawnAsteroid();
        }
    }

    // This function handles spawning a single asteroid at a random position.
    void SpawnAsteroid()
    {
        // Calculate the base spawn position in front of the player based on the specified spawn distance.
        Vector3 spawnPosition = playerTransform.position + playerTransform.forward * spawnDistance;

        // Add randomness to the X, Y, and Z coordinates to spread out the asteroids in the spawn area.
        spawnPosition.x += Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);

        spawnPosition.y += Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);

        spawnPosition.z += Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);

        // Ensure the asteroid is spawned far enough away, out of sight (adjust this based on your camera setup).
        spawnPosition.z += spawnDistance;

        // Create the asteroid prefab at the calculated spawn position.
        Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
    }
}

