using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        public GameObject gamePanel; // Menu panel
        public GameObject levelCompletePanel; // Menu panel
        public Slider loadingSlider; // Slider for loading progress


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

