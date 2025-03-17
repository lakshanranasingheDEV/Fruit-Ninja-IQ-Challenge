using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yunash.Game;
using Yunash.Audio;



namespace Yunash.UI
{
    public class LoginCanvasLoop : CanvasBase
    {
        public static LoginCanvasLoop Instance;
        private AudioManager audioManager;

        public GameObject gamePanel;

        public GameObject gameOverPanel;
        public GameObject pausePanel;


       
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
            gamePanel.SetActive(true);
            gameOverPanel.SetActive(false);
            pausePanel.SetActive(false);
            audioManager = FindObjectOfType<AudioManager>();

           
        }


        public void OnPresseHomeButton()
        {
            SceneManager.LoadScene("MainGameScene");
        }



        private void SaveLevelProgress()
        {
           
            PlayerPrefs.Save();
        }

        private void LoadLevelProgress()
        {
            
        }

        private void UpdateLevelText()
        {
            // Update level text if needed
        }

      public void Restart()
        {
            SceneManager.LoadScene("FruitNinjaLoop");
        }
        
    

        private void SetFirstTimeFlag(bool isFirstTime)
        {
           
            PlayerPrefs.Save();
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

       

        private IEnumerator DeactivateGamePanelWithDelay()
        {
            yield return null; // Wait for one frame
            
            Debug.Log("GamePanel deactivated after delay.");
        }

        

    }
}
