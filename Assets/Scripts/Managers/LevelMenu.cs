using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    public GameObject levelButtons;
    private int unlockedLevel;

    private void Awake()
    {
        ButtonsToArray();
        unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = i < unlockedLevel;
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

    void ButtonsToArray()
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
