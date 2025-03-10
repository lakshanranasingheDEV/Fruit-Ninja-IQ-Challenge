using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yunash.UI;
using Yunash.Audio;
using UnityEngine.SceneManagement;



public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;
    private AudioManager audioManager;


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
    public Button restartButton;

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
        InitializeGame();

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonClick);
        }
        else
        {
            Debug.LogWarning("Next Button is not assigned in the inspector.");
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartButtonClick);
        }
        else
        {
            Debug.LogWarning("Restart Button is not assigned in the inspector.");
        }
    }

    private void InitializeGame()
    {
        DeactivateAllLevels();
        ActivateLevelGameObject(currentLevel);
        SetRandomTask();
        UpdateLevelText();
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
            audioManager?.StopAudio(Yunash.Audio.AudioType.IdleBackgroundMusic);

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

        SaveGameProgress();
        ActivateLevelGameObject(currentLevel, false);
        currentLevel++;

        if (LoginCanvas.Instance != null && LoginCanvas.Instance.levelCompletePanel != null)
        {
            LoginCanvas.Instance.levelCompletePanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("LoginCanvas or levelCompletePanel is not set!");
        }

        if (currentLevel > levels.Count)
        {
            Debug.Log("All levels completed!");
           
            return;
        }

        ActivateLevelGameObject(currentLevel);
        
        SaveGameProgress();
        UpdateLevelText();
        SetRandomTask();
    }

    private void OnRestartButtonClick()
    {
        // Hide the level complete panel if it's active
        if (LoginCanvas.Instance != null && LoginCanvas.Instance.levelCompletePanel != null)
        {
            LoginCanvas.Instance.levelCompletePanel.SetActive(false);
            LoginCanvas.Instance.menuPanel.SetActive(false);
            LoginCanvas.Instance.gamePanel.SetActive(true);

        }
        else
        {
            Debug.LogWarning("LoginCanvas or levelCompletePanel is not assigned.");
        }
        SceneManager.LoadScene("MainGameScene");
        SceneManager.sceneLoaded += OnSceneLoaded;
        LoadGameProgress();  

        InitializeGame();
       

        Time.timeScale = 1f; 
        Debug.Log($"Level {currentLevel} restarted!");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {


        SceneManager.sceneLoaded -= OnSceneLoaded;
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

    private void DeactivateAllLevels()
    {
        foreach (var level in levels)
        {
            if (level.levelGameObject != null)
            {
                level.levelGameObject.SetActive(false);
            }
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
