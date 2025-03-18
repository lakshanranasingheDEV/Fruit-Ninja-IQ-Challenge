using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yunash.Game;
using Yunash.Audio;



namespace Yunash.UI
{
    public class LoginCanvas : CanvasBase
    {
        public static LoginCanvas Instance;
        private AudioManager audioManager;


        public GameObject gameOverPanel;
        public GameObject loadingPanel;
        public GameObject menuPanel;
        public GameObject gamePanel;
        public GameObject levelCompletePanel;
        public GameObject NoLivesPanel;
        public GameObject pausePanel;
        public GameObject settingPanel;
        public Slider loadingSlider;

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

            audioManager = FindObjectOfType<AudioManager>();

            gameOverPanel.SetActive(false);
            gamePanel.SetActive(false);
            levelCompletePanel.SetActive(false);
            NoLivesPanel.SetActive(false);
            menuPanel.SetActive(false);
            pausePanel.SetActive(false);
            settingPanel.SetActive(false);

            // Check if it's the first time launching the game
            if (IsFirstTime())
            {
                StartCoroutine(LoadGame());
            }
            else
            {
                menuPanel.SetActive(true);
                audioManager?.PlayAudio(Yunash.Audio.AudioType.IdleBackgroundMusic);

            }
        }

        public void OnStartButtonPressed()
        {
            int lives = PlayerPrefs.GetInt("PlayerLives", 3);

            // Show NoLivesPanel if lives are 0
            if (lives == 0)
            {
                gamePanel.SetActive(false);
                pausePanel.SetActive(false);
                ShowNoLivesPanel();
            }

            // Proceed to game panel if lives are 0 to 3
            if (lives <= 3)
            {
                audioManager?.StopAudio(Yunash.Audio.AudioType.IdleBackgroundMusic);

                menuPanel.SetActive(false);
                pausePanel.SetActive(false);
                gamePanel.SetActive(true);
                UpdateLevelText();
            }
        }

        public void OnPressedHomeButton()
        {
            menuPanel.SetActive(true);
            gameOverPanel.SetActive(false);
            gamePanel.SetActive(false);
            levelCompletePanel?.SetActive(false);
        }

        public void OnPressedChessPlayButton()
        {
            SceneManager.LoadScene("Chess");
        }

        public void OnPressedChessLoopPlayButton()
        {
            SceneManager.LoadScene("Chess Loop");
        }

        public void OnPresseFruitNinjaLoopPlayButton()
        {
            SceneManager.LoadScene("FruitNinjaLoop");
        }

        public void OnPresseHomeButton()
        {
            SceneManager.LoadScene("MainGameScene");
        }

        public void CompleteLevel()
        {
            audioManager?.PlayAudio(Yunash.Audio.AudioType.LevelComplete);

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
            // Update level text if needed
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

        private void ShowNoLivesPanel()
        {
            if (NoLivesPanel != null)
            {
                Debug.Log("Activating NoLivesPanel and deactivating gamePanel.");
                //StartCoroutine(DeactivateGamePanelWithDelay());
                audioManager?.PlayAudio(Yunash.Audio.AudioType.Error);

                gamePanel.SetActive(false);
                NoLivesPanel.SetActive(true);
            }
            else
            {
                Debug.LogError("NoLivesPanel is not assigned in the LoginCanvas!");
            }
            Time.timeScale = 0f;
        }

        public void openPauseButton()
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);

        }
        public void continueButton()
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);

        }
        public void OpensettingButton()
        {
            Time.timeScale = 0f;
            settingPanel.SetActive(true);

        }
        
        public void ClosesettingButton()
        {
            Time.timeScale = 1f;
            settingPanel.SetActive(false);

        }

        private IEnumerator DeactivateGamePanelWithDelay()
        {
            yield return null; // Wait for one frame
            
            Debug.Log("GamePanel deactivated after delay.");
        }

        

    }
}
