using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawnerLoop : MonoBehaviour
{
    public static FruitSpawnerLoop Instance; // Singleton reference

    public GameObject[] fruitPrefabs;
    public GameObject bombPrefab;
    public Transform[] spawnPoints;

    public float minDelay = 0.5f;
    public float maxDelay = 1.5f;

    private int fruitCounter = 0;
    public float bombSpawnChance = 0.7f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnFruits());
    }

    IEnumerator SpawnFruits()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];

            GameObject prefabToSpawn;

            if (fruitCounter < 5)
            {
                int fruitIndex = Random.Range(0, fruitPrefabs.Length);
                prefabToSpawn = fruitPrefabs[fruitIndex];
                fruitCounter++;
            }
            else
            {
                if (Random.value < bombSpawnChance)
                {
                    prefabToSpawn = bombPrefab;
                }
                else
                {
                    int fruitIndex = Random.Range(0, fruitPrefabs.Length);
                    prefabToSpawn = fruitPrefabs[fruitIndex];
                }
                fruitCounter = 0;
            }

            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
            Destroy(spawnedObject, 5f);
        }
    }

    public void IncreaseSpawnSpeed()
    {
        minDelay = Mathf.Max(0.1f, minDelay - 0.05f);
        maxDelay = Mathf.Max(0.3f, maxDelay - 0.1f);
        Debug.Log($"Spawn Speed Increased: Min Delay {minDelay}, Max Delay {maxDelay}");
    }
}
