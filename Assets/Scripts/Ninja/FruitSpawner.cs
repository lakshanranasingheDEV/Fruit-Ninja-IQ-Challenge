/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject[] fruitPrefabs; // Array to hold multiple fruit prefabs
    public GameObject bombPrefab;    // Bomb prefab
    public Transform[] spawnPoints;   // Array of spawn points

    public float minDelay = 0.1f;     // Minimum delay between spawns
    public float maxDelay = 1.0f;     // Maximum delay between spawns

    private int fruitCounter = 0;     // Counter to track fruits spawned

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

            // Randomly pick a spawn point
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];

            GameObject prefabToSpawn;

            if (fruitCounter < 5)
            {
                // Spawn a fruit
                int fruitIndex = Random.Range(0, fruitPrefabs.Length);
                prefabToSpawn = fruitPrefabs[fruitIndex];
                fruitCounter++; // Increment the fruit counter
            }
            else
            {
                // Spawn a bomb after 5 fruits
                prefabToSpawn = bombPrefab;
                fruitCounter = 0; // Reset the counter
            }

            // Spawn the chosen prefab
            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

            // Destroy the spawned object after 5 seconds
            Destroy(spawnedObject, 5f);
        }
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject[] fruitPrefabs; // Array to hold multiple fruit prefabs
    public GameObject bombPrefab;    // Bomb prefab
    public Transform[] spawnPoints;   // Array of spawn points

    public float minDelay = 0.1f;     // Minimum delay between spawns
    public float maxDelay = 1.0f;     // Maximum delay between spawns

    private int fruitCounter = 0;     // Counter to track fruits spawned
    public float bombSpawnChance = 0.7f; // Probability of spawning a bomb (0.0 to 1.0)

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

            // Randomly pick a spawn point
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];

            GameObject prefabToSpawn;

            if (fruitCounter < 5)
            {
                // Spawn a fruit
                int fruitIndex = Random.Range(0, fruitPrefabs.Length);
                prefabToSpawn = fruitPrefabs[fruitIndex];
                fruitCounter++; // Increment the fruit counter
            }
            else
            {
                // After 5 fruits, decide whether to spawn a bomb based on the chance
                if (Random.value < bombSpawnChance)
                {
                    prefabToSpawn = bombPrefab; // Spawn a bomb
                }
                else
                {
                    // Spawn a fruit if bomb chance fails
                    int fruitIndex = Random.Range(0, fruitPrefabs.Length);
                    prefabToSpawn = fruitPrefabs[fruitIndex];
                }
                fruitCounter = 0; // Reset the counter
            }

            // Spawn the chosen prefab
            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

            // Destroy the spawned object after 5 seconds
            Destroy(spawnedObject, 5f);
        }
    }
}
