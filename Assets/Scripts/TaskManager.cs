using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public Text taskText; // Task count text
    public Image taskFruitImage; // Task fruit image
    public Sprite[] fruitSprites; // Array of fruit sprites (match tags with these)

    private string[] fruitTags = { "Watermelon", "Apple" }; // Tags of fruits
    private string targetFruit; // The target fruit for the current task
    private int taskCount; // The number of fruits required to complete the task
    private Dictionary<string, Sprite> fruitSpriteMap; // Maps fruit tags to sprites

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

        // Initialize the fruit-to-sprite mapping
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
        // Choose a random fruit and count
        targetFruit = fruitTags[Random.Range(0, fruitTags.Length)];
        taskCount = Random.Range(5, 15); // Set a random target count (5–15)

        // Update the task UI
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
        // Use CompareTag to check if the fruit's tag matches the target fruit
        if (fruit.CompareTag(targetFruit))
        {
            taskCount--;

            UpdateTaskUI();

            if (taskCount <= 0)
            {
                CompleteTask();
            }
        }
    }
    private void CompleteTask()
    {
        Debug.Log("Task Completed!");
        //SetRandomTask(); // Start a new task
    }

}
