using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yunash.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public Text taskText; // Task count text
    public Image taskFruitImage; // Task fruit image
    public Sprite[] fruitSprites;

    private string[] fruitTags = { "Watermelon", "Apple" };
    private string targetFruit;
    private int taskCount;
    private Dictionary<string, Sprite> fruitSpriteMap;

    public int currentLevel = 1;

    [System.Serializable]
    public class LevelConfig
    {
        public int levelNumber; // The level number
        public int minCount;   // Minimum task count for the level
        public int maxCount;   // Maximum task count for the level
        public GameObject levelGameObject; // GameObject associated with this level
    }

    public List<LevelConfig> levels; // List of level configurations
    public Button nextButton; // Next button on the level complete panel
   // public Text levelText;    // Text to display the current level

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
        UpdateLevelText();
        ActivateLevelGameObject(currentLevel);
        SetRandomTask();

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonClick);
        }
    }

    public void SetRandomTask()
    {
        targetFruit = fruitTags[Random.Range(0, fruitTags.Length)];

        LevelConfig levelConfig = levels.Find(l => l.levelNumber == currentLevel);

        if (levelConfig != null)
        {
            taskCount = Random.Range(levelConfig.minCount, levelConfig.maxCount + 1);
        }
        else
        {
            Debug.LogError($"No configuration found for level {currentLevel}");
            return;
        }

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

    private void UpdateLevelText()
    {
       /* if (levelText != null)
        {
            levelText.text = $"Level {currentLevel}";
        }
        else
        {
            Debug.LogWarning("Level Text is not assigned in the Inspector.");
        }*/
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
        if (LoginCanvas.Instance != null && LoginCanvas.Instance.levelCompletePanel != null)
        {
            LoginCanvas.Instance.levelCompletePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("LoginCanvas or levelCompletePanel is not set!");
        }

        Time.timeScale = 0f;
    }

    private void OnNextButtonClick()
    {
        Time.timeScale = 1f; // Resume the game

        // Deactivate the current level's GameObject
        ActivateLevelGameObject(currentLevel, false);

        currentLevel++;
        LoginCanvas.Instance.levelCompletePanel.SetActive(false);

        // Activate the next level's GameObject
        ActivateLevelGameObject(currentLevel);

        UpdateLevelText(); // Update the level text for the next level
        SetRandomTask();
    }

    private void ActivateLevelGameObject(int level, bool activate = true)
    {
        LevelConfig levelConfig = levels.Find(l => l.levelNumber == level);

        if (levelConfig != null && levelConfig.levelGameObject != null)
        {
            levelConfig.levelGameObject.SetActive(activate);
        }
        else
        {
            Debug.LogWarning($"No GameObject found for level {level}");
        }
    }
}
