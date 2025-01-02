using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yunash.Game;

public class Game : MonoBehaviour
{
    
    public GameObject chesspiece;

    public Slider moveSlider; // Reference to the slider
    public Text moveText;     // Reference to the text below the slider


    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    private string currentPlayer = "white";

    private bool gameOver = false;

    private int whiteMoveCount = 0;
    private int totalWhiteMoves = 0;
    private int blackMoveCount = 0;
    private int maxMoves = 5;

    private Dictionary<GameObject, int> whitePieceMoveCounts = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, int> blackPieceMoveCounts = new Dictionary<GameObject, int>();

    public string player; // Ensure this is public or [SerializeField] to inspect in Unity


    public GameObject startBox; 
    public GameObject endBox;   
    public string nextSceneName; 
    public GameObject gameOverUI;
    public GameObject settingsPanel;

    private void Awake()
    {
        
        startBox.SetActive(true);
        endBox.SetActive(false);
        gameOverUI.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 0f; 
    }

    
    public void StartGame()
    {
        startBox.SetActive(false);
        Time.timeScale = 1f; 
    }

    
    public void EndGame()
    {
        Time.timeScale = 0f; 
        endBox.SetActive(true);
    }

    public void ToggleSettingsPanel()
    {
        if (settingsPanel != null)
        {
            bool isActive = settingsPanel.activeSelf;
            settingsPanel.SetActive(!isActive); // Toggle the panel visibility
        }
        else
        {
            Debug.LogWarning("Settings panel is not assigned in the inspector!");
        }
    }
    public void GoToNextScene()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainGameScene");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainGameScene" && NinjaManager.Instance != null)
        {
            NinjaManager.Instance.RestoreAllLives();
            Debug.Log("Lives restored without affecting the score.");
        }
        else
        {
            Debug.LogWarning("NinjaManager is not initialized or scene mismatch!");
        }

        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }

    //

    public void Start()
    {
        string[] whitePieceOptions = { "white_rook", "white_knight", "white_bishop", "white_queen", "white_king", "white_pawn" };
        playerWhite = RandomlyPlacePieces(whitePieceOptions, 3, "white");

        
        string[] blackPieceOptions = { "black_rook", "black_knight", "black_bishop", "black_queen", "black_king", "black_pawn" };
        playerBlack = RandomlyPlacePieces(blackPieceOptions, 1, "black");

        foreach (var piece in playerWhite)
        {
            whitePieceMoveCounts[piece] = 0; // Initialize movement counts for white pieces
        }

        moveSlider.maxValue = 5; // Set the slider max value
        moveSlider.value = 0; // Initialize slider value
        //UpdateMoveUI();
    }

    void InitializePiece()
    {
        Debug.Log($"Initializing piece: {gameObject.name}, Player: {player}");
    }



    private GameObject[] RandomlyPlacePieces(string[] pieceOptions, int count, string playerColor)
    {
        List<GameObject> placedPieces = new List<GameObject>();
        HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();

        for (int i = 0; i < count; i++)
        {
            string pieceName = pieceOptions[Random.Range(0, pieceOptions.Length)];

            
            Vector2 position;
            do
            {
                int x = Random.Range(0, 8);
                int y = Random.Range(playerColor == "white" ? 0 : 6, playerColor == "white" ? 2 : 8); 
                position = new Vector2(x, y);
            } while (occupiedPositions.Contains(position));

            occupiedPositions.Add(position);

            
            GameObject piece = Create(pieceName, (int)position.x, (int)position.y);
            placedPieces.Add(piece);
            SetPosition(piece);
        }

        return placedPieces.ToArray();
    }


    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>();
        cm.name = name;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        return x >= 0 && y >= 0 && x < 8 && y < 8;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void NextTurn()
    {
        currentPlayer = currentPlayer == "white" ? "black" : "white";

        // AI Turn Triggered
        if (currentPlayer == "black")
        {
            StartCoroutine(HandleAIMove());
        }
    }

    private IEnumerator HandleAIMove()
    {
        yield return new WaitForSeconds(1.0f); 

        List<GameObject> blackPieces = new List<GameObject>(playerBlack); 
        bool moveMade = false;

        while (!moveMade && blackPieces.Count > 0)
        {
            int randIndex = Random.Range(0, blackPieces.Count);
            GameObject piece = blackPieces[randIndex];

            if (piece != null)
            {
                var chessman = piece.GetComponent<Chessman>();
                if (chessman != null)
                {
                    Debug.Log($"AI handling move for {chessman.name}.");
                    chessman.InitiateMovePlates();
                    List<Vector2> validMoves = CollectValidMovesFromPlates();
                    Debug.Log($"Valid moves for {chessman.name}: {validMoves.Count}");

                    if (validMoves.Count > 0)
                    {
                        int moveIndex = Random.Range(0, validMoves.Count);
                        Vector2 target = validMoves[moveIndex];

                        Debug.Log($"{chessman.name} moving to {target}.");
                        MovePiece(piece, (int)target.x, (int)target.y);
                        moveMade = true;

                        if (blackPieceMoveCounts.ContainsKey(piece))
                        {
                            blackPieceMoveCounts[piece]++;
                        }
                        else
                        {
                            blackPieceMoveCounts[piece] = 1;
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"No valid moves for {chessman.name}. Removing piece.");

                        LogAndRemovePiece(piece, blackPieces, randIndex);
                    }

                    ClearMovePlates();
                }
                else
                {
                    Debug.LogError("Chessman component missing on the selected piece.");
                    blackPieces.RemoveAt(randIndex);
                }
            }
            else
            {
                LogAndRemovePiece(piece, blackPieces, randIndex);
            }

            if (!AreBlackPiecesRemaining())
            {
                Debug.Log("No black pieces remaining. Checking move count.");
                foreach (var entry in blackPieceMoveCounts)
                {
                    if (entry.Key != null)
                    {
                        Debug.Log($"Black piece {entry.Key.name} moved {entry.Value} times before elimination.");
                    }
                }
                Winner("white");
                yield break;
            }
        }

        NextTurn();
    }


    private void LogAndRemovePiece(GameObject piece, List<GameObject> blackPieces, int index)
    {
        if (piece != null && blackPieceMoveCounts.ContainsKey(piece))
        {
            Debug.Log($"Black piece {piece.name} moved {blackPieceMoveCounts[piece]} times before being eliminated.");
            blackPieceMoveCounts.Remove(piece);
        }
        else
        {
            Debug.LogWarning("Attempted to log a destroyed or null piece.");
        }

        blackPieces.RemoveAt(index);
    }



    private List<Vector2> CollectValidMovesFromPlates()
    {
        List<Vector2> validMoves = new List<Vector2>();

        foreach (GameObject movePlate in GameObject.FindGameObjectsWithTag("MovePlate"))
        {
            MovePlate mpScript = movePlate.GetComponent<MovePlate>();
            validMoves.Add(new Vector2(mpScript.GetX(), mpScript.GetY()));
        }

        return validMoves;
    }


    private List<Vector2> CheckValidMoves(GameObject piece)
    {
        List<Vector2> validMoves = new List<Vector2>();
        Chessman cm = piece.GetComponent<Chessman>();

        
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (cm.ValidMove(x, y)) 
                {
                    validMoves.Add(new Vector2(x, y));
                }
            }
        }
        return validMoves;
    }

    
    private bool AreBlackPiecesRemaining()
    {
        foreach (var piece in playerBlack)
        {
            if (piece != null) 
            {
                return true;
            }
        }
        return false;
    }

    public void MovePiece(GameObject piece, int x, int y)
    {
        if (piece == null)
        {
            Debug.LogError("MovePiece: The piece is null!");
            return;
        }

        Chessman cm = piece.GetComponent<Chessman>();
        if (cm == null)
        {
            Debug.LogError("MovePiece: Chessman component not found!");
            return;
        }

        Debug.Log($"Piece name: {piece.name}, Player: {cm.player}, Position: ({x}, {y})");


        Debug.Log($"Player of piece: {cm.player}");

        // Increment move count if the piece is white
        if (cm.player == "white")
        {
            Debug.Log($"Before increment: totalWhiteMoves = {totalWhiteMoves}");
            totalWhiteMoves++;
            moveSlider.value = totalWhiteMoves;
            UpdateMoveUI();
            Debug.Log($"After increment: totalWhiteMoves = {totalWhiteMoves}");
        }

        totalWhiteMoves++;

        // Update position logic
        GameObject target = GetPosition(x, y);
        if (target != null)
        {
            Chessman targetCm = target.GetComponent<Chessman>();
            if (targetCm != null && targetCm.player == "black")
            {
                Destroy(target);
                Debug.Log($"White eliminated black piece at position ({x}, {y}).");
            }
        }

        SetPositionEmpty(cm.GetXBoard(), cm.GetYBoard());
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.SetCoords();
        SetPosition(piece);

        Debug.Log($"Piece moved to new position ({x}, {y}). Total white moves: {totalWhiteMoves}");

        // Trigger Game Over if conditions are met
        if (totalWhiteMoves >= 5)
        {
            TriggerGameOver();
        }
    }

    public void UpdateMoveUI()
    {
        if (moveSlider != null)
        {
            moveSlider.value = totalWhiteMoves;
        }

        if (moveText != null)
        {
            moveText.text = $"Moves: {totalWhiteMoves}/{(int)moveSlider.maxValue}";
        }

        Debug.Log($"Slider updated: {moveSlider.value}, Text updated: {moveText.text}");
    }

    public void LogWhiteMoves()
    {
        foreach (var entry in whitePieceMoveCounts)
        {
            if (entry.Key != null)
            {
                Debug.Log($"White piece {entry.Key.name} moved {entry.Value} times.");
            }
        }
    }


    private void TriggerGameOver()
    {
        Debug.Log("Game Over! Player failed to eliminate the black chess piece.");
        Time.timeScale = 0f; 
        gameOverUI.SetActive(true); 
    }

    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        Time.timeScale = 1f; // Resume the game
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;
        Debug.Log(playerWinner + " is the winner!");

        if (playerWinner == "white")
        {

            Debug.Log("White eliminated black in " + whiteMoveCount + " moves!");
            EndGame(); 
        }
    }

    private void ClearMovePlates()
    {
        foreach (GameObject movePlate in GameObject.FindGameObjectsWithTag("MovePlate"))
        {
            Destroy(movePlate);
        }
    }

}

