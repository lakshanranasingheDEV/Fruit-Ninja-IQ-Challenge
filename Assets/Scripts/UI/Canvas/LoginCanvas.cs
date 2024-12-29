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

        public GameObject gameOverPanel; 
        public GameObject loadingPanel; 
        public GameObject menuPanel; 
        public GameObject gamePanel; 
        public GameObject levelCompletePanel; 
        public Slider loadingSlider; 
        //public Text currentLevelText;  

        private int currentLevel; 

        private const string CurrentLevelKey = "CurrentLevel"; 
        private const string IsFirstTimeKey = "IsFirstTime"; 

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

            // Check if it's the first time launching the game
            if (IsFirstTime())
            {
                StartCoroutine(LoadGame()); 
            }
            else
            {
                menuPanel.SetActive(true); 
            }
        }

        public void OnStartButtonPressed()
        {
            menuPanel.SetActive(false); 
            gamePanel.SetActive(true); 
            UpdateLevelText();
        }

        public void OnPressedHomeButton()
        {
            menuPanel.SetActive(true);
            gameOverPanel.SetActive(false);
        }

        public void OnPressedChessPlayButton()
        {
           
            SceneManager.LoadScene("Chess");
        }

        public void CompleteLevel()
        {
            currentLevel++; 
            SaveLevelProgress(); 
            ShowLevelCompletePanel();
        }

        private void SaveLevelProgress()
        {
            PlayerPrefs.SetInt(CurrentLevelKey, currentLevel);
            PlayerPrefs.Save();
        }

        private void LoadLevelProgress()
        {
            
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
            Time.timeScale = 0f; 
        }

        private IEnumerator LoadGame()
        {
            loadingPanel.SetActive(true);
            loadingSlider.value = 0;

            float progress = 0f;
            while (progress < 1f)
            {
                progress += 0.1f; 
                loadingSlider.value = progress;
                yield return new WaitForSeconds(0.1f);
            }

            loadingPanel.SetActive(false); 
            menuPanel.SetActive(true); 

            SetFirstTimeFlag(false); 
        }

        private bool IsFirstTime()
        {
            
            return PlayerPrefs.GetInt(IsFirstTimeKey, 1) == 1;
        }

        private void SetFirstTimeFlag(bool isFirstTime)
        {
            PlayerPrefs.SetInt(IsFirstTimeKey, isFirstTime ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}
