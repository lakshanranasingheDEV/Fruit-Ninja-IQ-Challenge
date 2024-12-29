using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yunash.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public Text taskText;
    public Image taskFruitImage;
    public Sprite[] fruitSprites;

    private string[] fruitTags = { "Watermelon", "Apple" };
    private string targetFruit;
    private int taskCount;
    private Dictionary<string, Sprite> fruitSpriteMap;

    public int currentLevel = 1;
    private const string CurrentLevelKey = "CurrentLevel";

    [System.Serializable]
    public class LevelConfig
    {
        public int levelNumber;
        public int minCount;
        public int maxCount;
        public GameObject levelGameObject;
    }

    public List<LevelConfig> levels;
    public Button nextButton;

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

        LoadGameProgress();
    }

    private void Start()
    {
        //PlayerPrefs.DeleteAll(); 
        UpdateLevelText();
        ActivateLevelGameObject(currentLevel);
        SetRandomTask();

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonClick);
        }
        else
        {
            Debug.LogWarning("Next Button is not assigned in the inspector.");
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
        Debug.Log($"Current Level: {currentLevel}");
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
        Time.timeScale = 1f;

        ActivateLevelGameObject(currentLevel, false);

        currentLevel++;
        SaveGameProgress();
        LoginCanvas.Instance.levelCompletePanel.SetActive(false);

        ActivateLevelGameObject(currentLevel);

        UpdateLevelText();
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

    private void SaveGameProgress()
    {
        PlayerPrefs.SetInt(CurrentLevelKey, currentLevel);
        PlayerPrefs.Save();
    }

    private void LoadGameProgress()
    {
        currentLevel = PlayerPrefs.GetInt(CurrentLevelKey, 1);
    }
}
