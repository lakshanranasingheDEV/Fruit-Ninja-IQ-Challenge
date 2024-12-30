

// the correct one

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    // Reference from Unity IDE
    public GameObject chesspiece;

    // Matrices needed
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    // Current turn
    private string currentPlayer = "white";

    // Game Ending
    private bool gameOver = false;

    public void Start()
    {
        // Initialize white pieces
        playerWhite = new GameObject[] { Create("white_rook", 0, 0), Create("white_knight", 1, 0),
            Create("white_bishop", 2, 0), Create("white_queen", 3, 0), Create("white_king", 4, 0),
            Create("white_bishop", 5, 0), Create("white_knight", 6, 0), Create("white_rook", 7, 0),
            Create("white_pawn", 0, 1), Create("white_pawn", 1, 1), Create("white_pawn", 2, 1),
            Create("white_pawn", 3, 1), Create("white_pawn", 4, 1), Create("white_pawn", 5, 1),
            Create("white_pawn", 6, 1), Create("white_pawn", 7, 1) };

        // Initialize black pieces
        playerBlack = new GameObject[] { Create("black_rook", 0, 7), Create("black_knight", 1, 7),
            Create("black_bishop", 2, 7), Create("black_queen", 3, 7), Create("black_king", 4, 7),
            Create("black_bishop", 5, 7), Create("black_knight", 6, 7), Create("black_rook", 7, 7),
            Create("black_pawn", 0, 6), Create("black_pawn", 1, 6), Create("black_pawn", 2, 6),
            Create("black_pawn", 3, 6), Create("black_pawn", 4, 6), Create("black_pawn", 5, 6),
            Create("black_pawn", 6, 6), Create("black_pawn", 7, 6) };

        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
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
        yield return new WaitForSeconds(1.0f); // Delay for AI thinking simulation

        List<GameObject> blackPieces = new List<GameObject>(playerBlack); // List of all black pieces

        bool moveMade = false;

        while (!moveMade && blackPieces.Count > 0)
        {
            int randIndex = Random.Range(0, blackPieces.Count);
            GameObject piece = blackPieces[randIndex];

            // Call InitiateMovePlates() for the selected piece to determine valid moves
            piece.GetComponent<Chessman>().InitiateMovePlates();

            // Retrieve valid move positions from the move plates
            List<Vector2> validMoves = CollectValidMovesFromPlates();

            if (validMoves.Count > 0)
            {
                // Pick a random valid move
                int moveIndex = Random.Range(0, validMoves.Count);
                Vector2 target = validMoves[moveIndex];

                // Move the piece to the selected target
                MovePiece(piece, (int)target.x, (int)target.y);
                moveMade = true;
            }
            else
            {
                // Remove the piece from the list if it has no valid moves
                blackPieces.RemoveAt(randIndex);
            }

            // Clear move plates to reset the board UI
            ClearMovePlates();
        }

        // Switch back to the player's turn
        NextTurn();
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

        // Iterate through all positions on the board to check valid moves
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (cm.ValidMove(x, y)) // Assuming 'ValidMove' is implemented in Chessman.cs
                {
                    validMoves.Add(new Vector2(x, y));
                }
            }
        }
        return validMoves;
    }

    public void MovePiece(GameObject piece, int x, int y)
    {
        Chessman cm = piece.GetComponent<Chessman>();

        SetPositionEmpty(cm.GetXBoard(), cm.GetYBoard());
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.SetCoords();
        SetPosition(piece);
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;
        Debug.Log(playerWinner + " is the winner!");
    }

    private void ClearMovePlates()
    {
        foreach (GameObject movePlate in GameObject.FindGameObjectsWithTag("MovePlate"))
        {
            Destroy(movePlate);
        }
    }

}
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    // Reference from Unity IDE
    public GameObject chesspiece;

    // Matrices needed
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    // Current turn
    private string currentPlayer = "white";

    // Game Ending
    private bool gameOver = false;



    //UI Handling part
    public GameObject startBox; // Drag Start Box Panel here
    public GameObject endBox;   // Drag End Box Panel here
    public string nextSceneName; // Name of the next scene to load

    private void Awake()
    {
        // Ensure only the start box is active at the beginning
        startBox.SetActive(true);
        endBox.SetActive(false);
        Time.timeScale = 0f; // Pause the game initially
    }

    // Call this when "OK" button is pressed
    public void StartGame()
    {
        startBox.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    // Trigger the end game state
    public void EndGame()
    {
        Time.timeScale = 0f; // Pause the game
        endBox.SetActive(true);
    }

    // Call this when the "Next Scene" button is pressed
    public void GoToNextScene()
    {
        Time.timeScale = 1f; // Ensure time scale is reset
        SceneManager.LoadScene(nextSceneName);
    }

    //

    public void Start()
    {
        // Randomly select and place 3 white pieces
        string[] whitePieceOptions = { "white_rook", "white_knight", "white_bishop", "white_queen", "white_king", "white_pawn" };
        playerWhite = RandomlyPlacePieces(whitePieceOptions, 3, "white");

        // Randomly select and place 1 black piece
        string[] blackPieceOptions = { "black_rook", "black_knight", "black_bishop", "black_queen", "black_king", "black_pawn" };
        playerBlack = RandomlyPlacePieces(blackPieceOptions, 1, "black");
    }

    // Helper function to randomly place pieces
    private GameObject[] RandomlyPlacePieces(string[] pieceOptions, int count, string playerColor)
    {
        List<GameObject> placedPieces = new List<GameObject>();
        HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();

        for (int i = 0; i < count; i++)
        {
            string pieceName = pieceOptions[Random.Range(0, pieceOptions.Length)];

            // Find a random unoccupied position
            Vector2 position;
            do
            {
                int x = Random.Range(0, 8);
                int y = Random.Range(playerColor == "white" ? 0 : 6, playerColor == "white" ? 2 : 8); // White starts near the bottom, black near the top
                position = new Vector2(x, y);
            } while (occupiedPositions.Contains(position));

            occupiedPositions.Add(position);

            // Create and place the piece
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
        yield return new WaitForSeconds(1.0f); // Delay for AI thinking simulation

        List<GameObject> blackPieces = new List<GameObject>(playerBlack); // List of all black pieces

        bool moveMade = false;

        while (!moveMade && blackPieces.Count > 0)
        {
            int randIndex = Random.Range(0, blackPieces.Count);
            GameObject piece = blackPieces[randIndex];

            // Check if the piece is null (destroyed)
            if (piece != null)
            {
                // Safe to access the piece
                var chessman = piece.GetComponent<Chessman>();
                if (chessman != null)
                {
                    // Call InitiateMovePlates() for the selected piece to determine valid moves
                    chessman.InitiateMovePlates();

                    // Retrieve valid move positions from the move plates
                    List<Vector2> validMoves = CollectValidMovesFromPlates();

                    if (validMoves.Count > 0)
                    {
                        // Pick a random valid move
                        int moveIndex = Random.Range(0, validMoves.Count);
                        Vector2 target = validMoves[moveIndex];

                        // Move the piece to the selected target
                        MovePiece(piece, (int)target.x, (int)target.y);
                        moveMade = true;
                    }
                    else
                    {
                        // Remove the piece from the list if it has no valid moves
                        blackPieces.RemoveAt(randIndex);
                    }

                    // Clear move plates to reset the board UI
                    ClearMovePlates();
                }
                else
                {
                    Debug.LogError("Chessman component missing on the selected piece.");
                    blackPieces.RemoveAt(randIndex); // Remove invalid piece to avoid infinite loop
                }
            }
            else
            {
                Debug.LogWarning("Piece is null or has been destroyed.");
                Winner("white");
                blackPieces.RemoveAt(randIndex); // Remove destroyed piece from the list
            }
        }

        // Switch back to the player's turn
        NextTurn();
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

        // Iterate through all positions on the board to check valid moves
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (cm.ValidMove(x, y)) // Assuming 'ValidMove' is implemented in Chessman.cs
                {
                    validMoves.Add(new Vector2(x, y));
                }
            }
        }
        return validMoves;
    }

    // Add this method to check if any black pieces are left
    private bool AreBlackPiecesRemaining()
    {
        foreach (var piece in playerBlack)
        {
            if (piece != null) // Check if the piece still exists
            {
                return true;
            }
        }
        return false;
    }

    // Update the MovePiece method
    public void MovePiece(GameObject piece, int x, int y)
    {
        Chessman cm = piece.GetComponent<Chessman>();

        // Check if the target position has an opponent piece
        GameObject target = GetPosition(x, y);
        if (target != null)
        {
            Chessman targetCm = target.GetComponent<Chessman>();

            // If white attacks a black piece
            if (cm.player == "white" && targetCm.player == "black")
            {
                // Remove the black piece
                Destroy(target);
                SetPositionEmpty(x, y);

                // Check if all black pieces are eliminated
                if (!AreBlackPiecesRemaining())
                {
                    Winner("white");
                    return;
                }
            }
        }

        // Move the piece to the new position
        SetPositionEmpty(cm.GetXBoard(), cm.GetYBoard());
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.SetCoords();
        SetPosition(piece);
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
            EndGame(); // Show end box
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

/* Black attack part
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    // Reference from Unity IDE
    public GameObject chesspiece;

    // Matrices needed
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    // Current turn
    private string currentPlayer = "white";

    // Game Ending
    private bool gameOver = false;


    public void Start()
    {
        // Initialize white pieces
        playerWhite = new GameObject[] { Create("white_rook", 0, 0), Create("white_knight", 1, 0),
            Create("white_bishop", 2, 0), Create("white_queen", 3, 0), Create("white_king", 4, 0),
            Create("white_bishop", 5, 0), Create("white_knight", 6, 0), Create("white_rook", 7, 0),
            Create("white_pawn", 0, 1), Create("white_pawn", 1, 1), Create("white_pawn", 2, 1),
            Create("white_pawn", 3, 1), Create("white_pawn", 4, 1), Create("white_pawn", 5, 1),
            Create("white_pawn", 6, 1), Create("white_pawn", 7, 1) };

        // Initialize black pieces
        playerBlack = new GameObject[] { Create("black_rook", 0, 7), Create("black_knight", 1, 7),
            Create("black_bishop", 2, 7), Create("black_queen", 3, 7), Create("black_king", 4, 7),
            Create("black_bishop", 5, 7), Create("black_knight", 6, 7), Create("black_rook", 7, 7),
            Create("black_pawn", 0, 6), Create("black_pawn", 1, 6), Create("black_pawn", 2, 6),
            Create("black_pawn", 3, 6), Create("black_pawn", 4, 6), Create("black_pawn", 5, 6),
            Create("black_pawn", 6, 6), Create("black_pawn", 7, 6) };

        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
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
        yield return new WaitForSeconds(1.0f); // Delay for AI thinking simulation

        List<GameObject> blackPieces = new List<GameObject>(playerBlack); // List of all black pieces
        bool moveMade = false;

        while (!moveMade && blackPieces.Count > 0)
        {
            int randIndex = Random.Range(0, blackPieces.Count);
            GameObject piece = blackPieces[randIndex];

            // Check if the piece still exists and is valid
            if (piece == null || piece.GetComponent<Chessman>() == null)
            {
                blackPieces.RemoveAt(randIndex);
                continue; // Skip this iteration
            }

            // Call InitiateMovePlates() for the selected piece to determine valid moves
            piece.GetComponent<Chessman>().InitiateMovePlates();

            // Retrieve attackable positions (positions occupied by white pieces)
            List<Vector2> attackMoves = CollectAttackMoves(new List<GameObject>(playerWhite));

            if (attackMoves.Count > 0)
            {
                // If attack moves are available, perform the attack
                Vector2 target = attackMoves[0]; // Prioritize the first attackable position
                MovePiece(piece, (int)target.x, (int)target.y);
                moveMade = true;
            }
            else
            {
                // Retrieve valid move positions from the move plates
                List<Vector2> validMoves = CollectValidMovesFromPlates();

                if (validMoves.Count > 0)
                {
                    // Pick a random valid move
                    int moveIndex = Random.Range(0, validMoves.Count);
                    Vector2 target = validMoves[moveIndex];

                    // Move the piece to the selected target
                    MovePiece(piece, (int)target.x, (int)target.y);
                    moveMade = true;
                }
                else
                {
                    // Remove the piece from the list if it has no valid moves
                    blackPieces.RemoveAt(randIndex);
                }
            }

            // Clear move plates to reset the board UI
            ClearMovePlates();
        }

        // Switch back to the player's turn
        NextTurn();
    }

   

    private List<Vector2> CollectAttackMoves(List<GameObject> whitePieces)
    {
        List<Vector2> attackMoves = new List<Vector2>();

        foreach (GameObject movePlate in GameObject.FindGameObjectsWithTag("MovePlate"))
        {
            if (movePlate == null) continue; // Skip if the move plate was destroyed
            MovePlate mpScript = movePlate.GetComponent<MovePlate>();
            int targetX = mpScript.GetX();
            int targetY = mpScript.GetY();

            foreach (GameObject whitePiece in whitePieces)
            {
                if (whitePiece == null) continue; // Skip if the white piece was destroyed
                Chessman cm = whitePiece.GetComponent<Chessman>();

                if (cm.GetXBoard() == targetX && cm.GetYBoard() == targetY)
                {
                    GameObject targetPiece = GetPieceAtPosition(targetX, targetY);
                    if (targetPiece != null && IsBlackPiece(targetPiece))
                    {
                        // Highlight the attackable piece
                        HighlightPiece(targetPiece);

                        // Wait for player confirmation
                        StartCoroutine(WaitForPlayerConfirmation(targetPiece));

                        attackMoves.Add(new Vector2(targetX, targetY));
                    }
                    break;
                }
            }
        }

        return attackMoves;
    }

    // Method to get the piece at a specific board position
    private GameObject GetPieceAtPosition(int x, int y)
    {
        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("ChessPiece"))
        {
            Chessman cm = piece.GetComponent<Chessman>();
            if (cm.GetXBoard() == x && cm.GetYBoard() == y)
            {
                return piece;
            }
        }
        return null;
    }

    // Method to check if the piece is a black chess piece
    private bool IsBlackPiece(GameObject piece)
    {
        Chessman cm = piece.GetComponent<Chessman>();
        return cm != null && cm.IsBlack();
    }

    // Coroutine to wait for player confirmation
    private IEnumerator WaitForPlayerConfirmation(GameObject targetPiece)
    {
        bool confirmed = false;

        // Add a click listener to the target piece
        targetPiece.GetComponent<Chessman>().OnPieceClicked += () => confirmed = true;

        // Wait until the player clicks the target piece
        yield return new WaitUntil(() => confirmed);

        // Remove the click listener after confirmation
        targetPiece.GetComponent<Chessman>().OnPieceClicked -= () => confirmed = true;

        // Proceed with the attack
        HandleAttack(targetPiece);
    }

    // Highlight the piece to indicate it's attackable
    private void HighlightPiece(GameObject piece)
    {
        // Add your logic to visually indicate the piece is attackable
        piece.GetComponent<Renderer>().material.color = Color.red;
    }

    // Handle the attack logic
    private void HandleAttack(GameObject targetPiece)
    {
        // Implement your logic to handle the attack (e.g., remove the piece, update score)
        Destroy(targetPiece);
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

        // Iterate through all positions on the board to check valid moves
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (cm.ValidMove(x, y)) // Assuming 'ValidMove' is implemented in Chessman.cs
                {
                    validMoves.Add(new Vector2(x, y));
                }
            }
        }
        return validMoves;
    }

    public void MovePiece(GameObject piece, int x, int y)
    {
        Chessman cm = piece.GetComponent<Chessman>();

        SetPositionNull(cm.GetXBoard(), cm.GetYBoard());
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.SetCoords();
        SetPosition(piece);
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;
        Debug.Log(playerWinner + " is the winner!");
    }

    private void ClearMovePlates()
    {
        foreach (GameObject movePlate in GameObject.FindGameObjectsWithTag("MovePlate"))
        {
            Destroy(movePlate);
        }
    }

    public void SetPositionNull(int x, int y)
    {
        positions[x, y] = null; // Clear the reference to the destroyed piece
    }


}

*/
