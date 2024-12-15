/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject _fruitPrefab;
    public Transform[] spawnPoints;

    public float minDelay = 0.1f;
    public float maxDelay = 1.0f;

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

            GameObject spawnedFruit =  Instantiate(_fruitPrefab, spawnPoint.position, spawnPoint.rotation);
            
            Destroy(spawnedFruit, 5f );
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
    public Transform[] spawnPoints;   // Array of spawn points

    public float minDelay = 0.1f;     // Minimum delay between spawns
    public float maxDelay = 1.0f;     // Maximum delay between spawns

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

            // Randomly pick a fruit prefab
            int fruitIndex = Random.Range(0, fruitPrefabs.Length);
            GameObject fruitPrefab = fruitPrefabs[fruitIndex];

            // Spawn the fruit at the chosen spawn point
            GameObject spawnedFruit = Instantiate(fruitPrefab, spawnPoint.position, spawnPoint.rotation);

            // Destroy the fruit after 5 seconds
            Destroy(spawnedFruit, 5f);
        }
    }
}
