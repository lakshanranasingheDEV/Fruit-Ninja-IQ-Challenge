/*using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    public Text scoreText; // Reference to the UI Text for the score
    public Image[] lifeIcons; // Array of life UI icons
    public Sprite lostLifeSprite; // Sprite to display when a life is lost
    public GameObject gameOverPanel; // Reference to the Game Over UI

    private int score = 0;
    private int lives = 3; // Total number of lives

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
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
        UpdateScoreUI();
        UpdateLivesUI();
        gameOverPanel.SetActive(false); // Hide Game Over panel at the start
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    public void SubtractScore(int amount)
    {
        score -= amount;
        if (score < 0) score = 0; // Prevent negative score
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
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
        gameOverPanel.SetActive(true); // Show Game Over panel
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}

*/
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text scoreText;
    public Image[] lifeIcons; // Array of life UI icons
    public Sprite lostLifeSprite; // Sprite to display when a life is lost
    public GameObject gameOverPanel; // Reference to the Game Over UI

    private int score = 0;
    private int lives = 3; // Total number of lives

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
        UpdateScoreUI();
        UpdateLivesUI();
        gameOverPanel.SetActive(false); // Hide Game Over panel at the start
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
        scoreText.text = "Score: " + score;
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
        gameOverPanel.SetActive(true); // Show Game Over panel
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Game"); // Load the scene by name
    }

}
