using Yunash.Audio;
using Yunash.Data;
using Yunash.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Yunash.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private DataManager dataManager;

        public static GameManager Instance;
        public IUIService UIService;
        public IAudioService AudioService;
        public IDataService DataService;

        public Text scoreText;
        public Image[] lifeIcons; // Array of life UI icons
        public Sprite lostLifeSprite; // Sprite to display when a life is lost


        private int score = 0;
        private int lives = 3; // Total number of lives


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance);
                Instance = this;
            }
            else
                Instance = this;

            if (uiManager == null || audioManager == null || dataManager == null)
            {
                throw new NullReferenceException("GameManager: Initialization: One or More Managers are missing.");
            }

            UIService = uiManager;
            AudioService = audioManager;
            DataService = dataManager;
        }

        private void Start()
        {
            LoadGameData();
            UpdateScoreUI();
            UpdateLivesUI();
            
        }

        public void AddScore(int amount)
        {
            score += amount;
            UpdateScoreUI();


        }

        public void SubtractLife()
        {
            if (lives > 0)
            {
                lives--;
                UpdateLivesUI();

                if (lives <= 0)
                {
                    GameOver();
                }
            }
        }

        private void UpdateScoreUI()
        {
            scoreText.text = score.ToString();
        }

        private void UpdateLivesUI()
        {
            // Update the UI sprites for lives
            for (int i = 0; i < lifeIcons.Length; i++)
            {
                if (i < lives)
                {
                    lifeIcons[i].enabled = true; // Keep full lives visible
                }
                else
                {
                    lifeIcons[i].sprite = lostLifeSprite; // Change to lost life sprite
                }
            }
        }

        private void GameOver()
        {
            if (LoginCanvas.Instance != null && LoginCanvas.Instance.gameOverPanel != null)
            {
                LoginCanvas.Instance.gameOverPanel.SetActive(true); // Show Game Over panel
            }
            else
            {
                Debug.LogError("GameManager: GameOver: LoginCanvas or GameOverPanel is not set!");
            }

            Time.timeScale = 0f; // Pause the game
        }

        public void RestartGame()
        {
            Time.timeScale = 1f; // Resume the game
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main Game"); // Load the scene by name
        }

        private void SaveGameData()
        {
            // Create a SaveGameData object to store score and lives
            SaveGameData saveData = new SaveGameData(score, lives);

            // Save the game data using DataManager
            dataManager.SaveData(saveData, "gameData.json");
        }

        private void LoadGameData()
        {
            // Load the game data when the game starts
            if (dataManager.TryLoadData("gameData.json", out SaveGameData loadedData))
            {
                score = loadedData.score;

                lives = loadedData.lives;
            }
        }
    }
}
