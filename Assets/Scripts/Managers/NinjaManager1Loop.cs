using Yunash.Audio;
using Yunash.Data;
using Yunash.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Yunash.Game
{
    public class NinjaManager1Loop : MonoBehaviour
    {
        public static NinjaManager1Loop Instance;
        private AudioManager audioManager;

        public Image[] lifeIcons;
        public Sprite lostLifeSprite;

        [Header("Score Settings")]
        [SerializeField] private List<TextMeshProUGUI> scoreTexts; // Use TextMeshProUGUI instead of Text
        private int score = 0;

        private int lives;

        private const string ScoreKey = "PlayerScore";

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
            lives = lifeIcons.Length; // Always start with full lives
            LoadGameProgress();

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
        }

        public void SaveGameProgress()
        {
            PlayerPrefs.SetInt(ScoreKey, score);
            PlayerPrefs.Save();
            Debug.Log($"Game progress saved: Score = {score}");
        }

        private void GameOver()
        {
            if (LoginCanvasLoop.Instance != null && LoginCanvasLoop.Instance.gameOverPanel != null)
            {
                LoginCanvasLoop.Instance.gameOverPanel.SetActive(true);
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
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainGameScene");
        }

        private void LoadGameProgress()
        {
            score = PlayerPrefs.GetInt(ScoreKey, 0);
            Debug.Log($"Loaded score: {score}");
        }

        public void ResetGameProgress()
        {
            PlayerPrefs.DeleteKey(ScoreKey);
        }
    }
}
