using Yunash.Audio;
using Yunash.Data;
using Yunash.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

namespace Yunash.Game
{
    public class NinjaManager : MonoBehaviour
    {
        public static NinjaManager Instance;
        private AudioManager audioManager;

        public Image[] lifeIcons;
        public Sprite lostLifeSprite;

        [Header("Score Settings")]
        [SerializeField] private List<TextMeshProUGUI> scoreTexts; // Use TextMeshProUGUI instead of Text
        private int score = 0;

        private int lives = 3;

        private const string ScoreKey = "PlayerScore";
        private const string LivesKey = "PlayerLives";

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance);
                Instance = this;
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            LoadGameProgress();
            if (lives <= 0)
            {
                ShowNoLevelPanel();
            }

            UpdateScoreUI();
            UpdateLivesUI();
        }

        public void AddScore(int amount)
        {
            score += amount;
            SaveGameProgress();
            UpdateScoreUI();
        }

        public void SubtractLife()
        {
            if (lives > 0)
            {
                lives--;
                UpdateLivesUI();
                SaveGameProgress();

                if (lives <= 0)
                {
                    GameOver();
                }
            }
        }

        private void UpdateScoreUI()
        {
            foreach (TextMeshProUGUI scoreText in scoreTexts)
            {
                if (scoreText != null)
                {
                    scoreText.text = score.ToString();
                }
            }
        }

        private void UpdateLivesUI()
        {
            for (int i = 0; i < lifeIcons.Length; i++)
            {
                if (i < lives)
                {
                    lifeIcons[i].enabled = true;
                }
                else
                {
                    lifeIcons[i].sprite = lostLifeSprite;
                }
            }
        }

        public void RestoreAllLives()
        {
            lives = lifeIcons.Length;
            UpdateLivesUI();
            SaveGameProgress(false);
        }

        public void SaveGameProgress(bool saveScore = true)
        {
            PlayerPrefs.SetInt(LivesKey, lives);
            if (saveScore)
            {
                PlayerPrefs.SetInt(ScoreKey, score);
            }
            PlayerPrefs.Save();
            Debug.Log($"Game progress saved: Lives = {lives}, Score = {score} (Score saved: {saveScore})");
        }

        private void GameOver()
        {
            if (LoginCanvas.Instance != null && LoginCanvas.Instance.gameOverPanel != null)
            {
                LoginCanvas.Instance.gameOverPanel.SetActive(true);
                audioManager?.StopAudio(Yunash.Audio.AudioType.IdleBackgroundMusic);
            }
            else
            {
                Debug.LogError("GameManager: GameOver: LoginCanvas or GameOverPanel is not set!");
            }

            Time.timeScale = 0f;
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            ResetGameProgress();
            RestoreAllLives();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainGameScene");
        }

        private void SaveGameProgress()
        {
            PlayerPrefs.SetInt(ScoreKey, score);
            PlayerPrefs.SetInt(LivesKey, lives);
            PlayerPrefs.Save();
        }

        private void LoadGameProgress()
        {
            lives = PlayerPrefs.GetInt(LivesKey, lifeIcons.Length);
            Debug.Log($"Loaded lives: {lives}");
            score = PlayerPrefs.GetInt(ScoreKey, 0);
            Debug.Log($"Loaded score: {score}");
        }

        public void ResetGameProgress()
        {
            PlayerPrefs.DeleteKey(ScoreKey);
            PlayerPrefs.DeleteKey(LivesKey);
        }

        public void ShowNoLevelPanel()
        {
            if (LoginCanvas.Instance != null && LoginCanvas.Instance.NoLivesPanel != null)
            {
                LoginCanvas.Instance.NoLivesPanel.SetActive(true);
                audioManager?.StopAudio(Yunash.Audio.AudioType.IdleBackgroundMusic);
            }
            else
            {
                Debug.LogError("GameManager: NoLivesPanel: LoginCanvas or NoLivesPanel is not set!");
            }
        }
    }
}
