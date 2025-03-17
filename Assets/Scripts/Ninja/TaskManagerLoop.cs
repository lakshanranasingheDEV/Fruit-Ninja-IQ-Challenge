using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManagerLoop : MonoBehaviour
{
    public static TaskManagerLoop Instance;

    public Text taskText;
    public Image taskFruitImage;
    public Sprite[] fruitSprites;

    private string[] fruitTags = { "Watermelon", "Apple" };
    private string targetFruit;
    private int taskCount;
    private Dictionary<string, Sprite> fruitSpriteMap;

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

        fruitSpriteMap = new Dictionary<string, Sprite>();
        for (int i = 0; i < fruitTags.Length; i++)
        {
            fruitSpriteMap[fruitTags[i]] = fruitSprites[i];
        }
    }

    private void Start()
    {
        SetRandomTask();
    }

    public void SetRandomTask()
    {
        targetFruit = fruitTags[Random.Range(0, fruitTags.Length)];
        taskCount = Random.Range(3, 6); // Random count per task

        if (fruitSpriteMap.ContainsKey(targetFruit))
        {
            taskFruitImage.sprite = fruitSpriteMap[targetFruit];
        }
        UpdateTaskUI();
    }

    private void UpdateTaskUI()
    {
        taskText.text = taskCount.ToString();
    }

    public void DecrementTaskCount(GameObject fruit)
    {
        if (fruit.CompareTag(targetFruit))
        {
            taskCount--;
            UpdateTaskUI();

            if (taskCount <= 0)
            {
                LevelComplete();
            }
        }
    }

    private void LevelComplete()
    {
        Debug.Log("Task Completed!");

        // Increase spawn speed
        //FruitSpawnerLoop.Instance.IncreaseSpawnSpeed();
        FruitSpawnerLoop.Instance.IncreaseSpawnSpeed();

        // Assign a new task instantly
        SetRandomTask();
    }
}
