using System.Collections.Generic;
using UnityEngine;

public class RockSpawning : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] asteroidPrefabs;
    public GameObject[] collectablePrefabs;

    [Header("References")]
    public Transform playerTransform;

    [Header("Counts")]
    public int targetAsteroidCount = 40;
    public int targetCollectableCount = 10;

    [Header("Distances")]
    public float minSpawnDistance = 35f;
    public float maxSpawnDistance = 80f;
    public float despawnDistance = 100f;

    [Header("Distance Visual Scaling")]
    public float farScaleMultiplier = 0.15f;
    public float nearScaleMultiplier = 1f;

    [Header("Asteroids Only - Base Size Variation")]
    public float asteroidMinScale = 0.7f;
    public float asteroidMaxScale = 1.5f;

    private List<GameObject> asteroids = new List<GameObject>();
    private List<GameObject> collectables = new List<GameObject>();

    private Dictionary<GameObject, Vector3> baseScales = new Dictionary<GameObject, Vector3>();

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("RockSpawning: Player Transform is missing.");
            return;
        }

        FillList(asteroids, asteroidPrefabs, targetAsteroidCount, true);
        FillList(collectables, collectablePrefabs, targetCollectableCount, false);
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        CleanupAndRespawn(asteroids, asteroidPrefabs, targetAsteroidCount, true);
        CleanupAndRespawn(collectables, collectablePrefabs, targetCollectableCount, false);

        UpdateDistanceScaling();
    }

    void FillList(List<GameObject> objectList, GameObject[] prefabArray, int targetCount, bool isAsteroid)
    {
        while (objectList.Count < targetCount)
        {
            GameObject obj = SpawnObject(prefabArray, isAsteroid);
            if (obj == null)
                break;

            objectList.Add(obj);
        }
    }

    void CleanupAndRespawn(List<GameObject> objectList, GameObject[] prefabArray, int targetCount, bool isAsteroid)
    {
        for (int i = objectList.Count - 1; i >= 0; i--)
        {
            GameObject obj = objectList[i];

            if (obj == null)
            {
                objectList.RemoveAt(i);
                continue;
            }

            float distance = Vector3.Distance(playerTransform.position, obj.transform.position);

            if (distance > despawnDistance)
            {
                baseScales.Remove(obj);
                Destroy(obj);
                objectList.RemoveAt(i);
            }
        }

        while (objectList.Count < targetCount)
        {
            GameObject obj = SpawnObject(prefabArray, isAsteroid);
            if (obj == null)
                break;

            objectList.Add(obj);
        }
    }

    GameObject SpawnObject(GameObject[] prefabArray, bool isAsteroid)
    {
        if (prefabArray == null || prefabArray.Length == 0)
            return null;

        GameObject prefab = prefabArray[Random.Range(0, prefabArray.Length)];
        if (prefab == null)
            return null;

        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject spawnedObject = Instantiate(prefab, spawnPosition, Random.rotation);

        Vector3 baseScale = prefab.transform.localScale;

        if (isAsteroid)
        {
            float randomScaleMultiplier = Random.Range(asteroidMinScale, asteroidMaxScale);
            baseScale *= randomScaleMultiplier;
        }

        baseScales[spawnedObject] = baseScale;

        float initialScaleMultiplier = GetDistanceScaleMultiplier(
            Vector3.Distance(playerTransform.position, spawnedObject.transform.position)
        );

        spawnedObject.transform.localScale = baseScale * initialScaleMultiplier;

        return spawnedObject;
    }

    void UpdateDistanceScaling()
    {
        UpdateObjectListScaling(asteroids);
        UpdateObjectListScaling(collectables);
    }

    void UpdateObjectListScaling(List<GameObject> objectList)
    {
        for (int i = objectList.Count - 1; i >= 0; i--)
        {
            GameObject obj = objectList[i];

            if (obj == null)
                continue;

            if (!baseScales.ContainsKey(obj))
                continue;

            float distance = Vector3.Distance(playerTransform.position, obj.transform.position);
            float scaleMultiplier = GetDistanceScaleMultiplier(distance);

            obj.transform.localScale = baseScales[obj] * scaleMultiplier;
        }
    }

    float GetDistanceScaleMultiplier(float distance)
    {
        float t = 1f - Mathf.Clamp01(distance / despawnDistance);
        t = t * t;
        return Mathf.Lerp(farScaleMultiplier, nearScaleMultiplier, t);
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomDirection = Random.onUnitSphere;
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        return playerTransform.position + randomDirection * randomDistance;
    }

    void OnDrawGizmosSelected()
    {
        if (playerTransform == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerTransform.position, minSpawnDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(playerTransform.position, maxSpawnDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, despawnDistance);
    }
}