using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yunash.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    public GameObject levelButtons;

    private void Awake()
    {
        ButtonsToArray();

        // Check if TaskManager is initialized before accessing it
        if (TaskManager.Instance != null)
        {
            int currentLevel = TaskManager.Instance.currentLevel;

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = i < currentLevel; // Unlock up to current level
            }
        }
        else
        {
            Debug.LogError("TaskManager instance not found!");
        }

        DeactivateAllLevels();
    }

    public void OpenLevel(int levelId)
    {
        DeactivateAllLevels();

        if (TaskManager.Instance != null)
        {
            var levels = TaskManager.Instance.levels;
            if (levelId - 1 < levels.Count && levels[levelId - 1].levelGameObject != null)
            {
                levels[levelId - 1].levelGameObject.SetActive(true);
                TaskManager.Instance.currentLevel = levelId; // Update current level
                TaskManager.Instance.SetRandomTask();
                TaskManager.Instance.UpdateLevelText();

                // Activate game panel and deactivate mission panel if LoginCanvas is available
                if (LoginCanvas.Instance != null && LoginCanvas.Instance.gamePanel != null)
                {
                    LoginCanvas.Instance.gamePanel.SetActive(true);
                    LoginCanvas.Instance.MissionPanel.SetActive(false);
                }
                else
                {
                    Debug.LogError("LoginCanvas or levelCompletePanel is not set!");
                }

               
            }
            else
            {
                Debug.LogError("Invalid level index: " + levelId);
            }
        }
        else
        {
            Debug.LogError("TaskManager instance not found!");
        }
    }


    private void ButtonsToArray()
    {

        int childCount = levelButtons.transform.childCount;
        buttons = new Button[childCount];

        for (int i = 0; i < childCount; i++)
        {
            buttons[i] = levelButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
            int levelIndex = i + 1;
            buttons[i].onClick.AddListener(() => OpenLevel(levelIndex));

        }
    }

    private void DeactivateAllLevels()
    {
        if (TaskManager.Instance != null)
        {
            foreach (var level in TaskManager.Instance.levels)
            {
                if (level.levelGameObject != null)
                {
                    level.levelGameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("TaskManager instance not found!");
        }
    }
}
