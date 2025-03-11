using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButtonNumberUpdater : MonoBehaviour
{
    [SerializeField] private LevelMenu levelMenu;
    [SerializeField] private SceneLoader sceneLoader;
    private TMP_Text levelButtonText;
    private Button levelButton;
    private int levelIndex;

    void Start()
    {
        levelButtonText = GetComponentInChildren<TMP_Text>();
        levelButton = GetComponent<Button>();

        string buttonName = gameObject.name;
        int startIndex = buttonName.IndexOf("(");
        int endIndex = buttonName.IndexOf(")");

        if (startIndex != -1 && endIndex != -1)
        {
            string numberString = buttonName.Substring(startIndex + 1, endIndex - startIndex - 1);
            if (int.TryParse(numberString, out levelIndex))
            {
                levelButtonText.text = $"{levelIndex}";
                levelButton.onClick.AddListener(() => OpenLevel(levelIndex));
            }
        }
    }

    void OpenLevel(int levelId)
    {
        if (levelMenu != null)
        {
            levelMenu.OpenLevel(levelId);
        }
        else if (TaskManager.Instance != null) // Use TaskManager instead of SceneLoader
        {
            TaskManager.Instance.DeactivateAllLevels(); // Ensure only one level is active
            TaskManager.Instance.ActivateLevelGameObject(levelId);
            TaskManager.Instance.currentLevel = levelId;
            TaskManager.Instance.SetRandomTask(); // Set a new task for the level
            TaskManager.Instance.UpdateLevelText();
        }
        else
        {
            Debug.LogError("Neither LevelMenu nor TaskManager reference is set in LevelButtonNumberUpdater");
        }
    }

}