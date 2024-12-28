using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yunash.Game;

namespace Yunash.UI
{
    public class LoginCanvas : CanvasBase
    {
        public static LoginCanvas Instance;

        public GameObject gameOverPanel; // Reference to the Game Over UI
        public GameObject loadingPanel; // Loading page panel
        public GameObject menuPanel; // Menu panel
        public GameObject gamePanel; // Game panel
        public GameObject levelCompletePanel; // Level Complete panel
        public Slider loadingSlider; // Slider for loading progress
        //public Text currentLevelText; // Text to show the current level

        private int currentLevel; // Track the current level

        private const string CurrentLevelKey = "CurrentLevel"; // Key for saving current level

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
        }

        private void Start()
        {
            LoadLevelProgress();

            gameOverPanel.SetActive(false);
            gamePanel.SetActive(false);
            levelCompletePanel.SetActive(false);

            menuPanel.SetActive(false);
            StartCoroutine(LoadGame());
        }

        public void OnStartButtonPressed()
        {
            menuPanel.SetActive(false); // Hide menu panel
            gamePanel.SetActive(true); // Show game panel
            UpdateLevelText();
        }

        public void OnPressedHomeButton()
        {
            menuPanel.SetActive(true);
            gameOverPanel.SetActive(false);
        }

        public void OnPressedChessPlayButton()
        {
            // Load the Chess scene
            SceneManager.LoadScene("Chess");
        }

        public void CompleteLevel()
        {
            currentLevel++; // Increment level
            SaveLevelProgress(); // Save the current level
            ShowLevelCompletePanel();
        }

        private void SaveLevelProgress()
        {
            PlayerPrefs.SetInt(CurrentLevelKey, currentLevel);
            PlayerPrefs.Save();
        }

        private void LoadLevelProgress()
        {
            // Load the saved level or default to level 1
            currentLevel = PlayerPrefs.GetInt(CurrentLevelKey, 1);
        }

        private void UpdateLevelText()
        {
           /* if (currentLevelText != null)
            {
                currentLevelText.text = "Level: " + currentLevel.ToString();
            }*/
        }

        private void ShowLevelCompletePanel()
        {
            if (levelCompletePanel != null)
            {
                levelCompletePanel.SetActive(true);
            }
            Time.timeScale = 0f; // Pause the game
        }

        private IEnumerator LoadGame()
        {
            loadingPanel.SetActive(true);
            loadingSlider.value = 0;

            float progress = 0f;
            while (progress < 1f)
            {
                progress += 0.1f; // Simulate loading progress
                loadingSlider.value = progress;
                yield return new WaitForSeconds(0.1f);
            }

            loadingPanel.SetActive(false); // Hide loading panel
            menuPanel.SetActive(true); // Show menu panel
        }
    }
}
