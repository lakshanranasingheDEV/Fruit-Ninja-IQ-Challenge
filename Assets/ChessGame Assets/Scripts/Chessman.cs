

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    public GameObject board;

    private bool hasMoved = false;

    
    public GameObject controller;
    public GameObject movePlate;

   
    private int xBoard = -1;
    private int yBoard = -1;

    
    public string player;

    
    public Sprite black_queen, black_knight, black_bishop, black_king, black_rook, black_pawn;
    public Sprite white_queen, white_knight, white_bishop, white_king, white_rook, white_pawn;

    public void Activate()
    {
        board = GameObject.Find("Board"); 

       
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        

      
        switch (this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
        }
    }

    public void SetCoords()
    {
        
        float x = xBoard;
        float y = yBoard;

       
        x *= 0.56f;
        y *= 0.56f;

        
        x += -1.8f;
        y += -2.3f;

        this.transform.position = new Vector3(x -0.15f , y, -1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        Debug.Log("Piece clicked: " + this.name);
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();
            InitiateMovePlates();
        }
    }


    public void DestroyMovePlates()
    {
        
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]); 
        }
    }

    public void InitiateMovePlates()
    {
        Debug.Log("InitiateMovePlates called for: " + this.name);
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                Debug.Log("Queen movement triggered");
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "black_knight":
            case "white_knight":
                Debug.Log("Knight movement triggered");
                LMovePlate();
                break;
            case "black_bishop":
            case "white_bishop":
                Debug.Log("Bishop movement triggered");
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;
            case "black_king":
            case "white_king":
                Debug.Log("King movement triggered");
                SurroundMovePlate();
                break;
            case "black_rook":
            case "white_rook":
                Debug.Log("Rook movement triggered");
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            case "black_pawn":
                Debug.Log("Black Pawn movement triggered");
                PawnMovePlate(xBoard, yBoard - 1);
                break;
            case "white_pawn":
                Debug.Log("White Pawn movement triggered");
                PawnMovePlate(xBoard, yBoard + 1);
                break;
        }
    }


    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        Debug.Log(this.name + " trying to move in direction: " + xIncrement + "," + yIncrement);

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            Debug.Log("Move available at: " + x + ", " + y);
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null)
        {
            Debug.Log("Piece encountered at: " + x + ", " + y);
            if (sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }


    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);

        
        if (this.name == "white_king")
        {
            CastlingMovePlate();
        }
    }

    private void CastlingMovePlate()
    {
        Game sc = controller.GetComponent<Game>();

        if (hasMoved)
        {
            Debug.Log("King has already moved, castling not allowed.");
            return;
        }

        
        if (sc.CanCastle(xBoard, yBoard, "kingside", player))
        {
            Debug.Log("Kingside castling allowed.");
            MovePlateSpawn(xBoard + 2, yBoard);
        }

       
        if (sc.CanCastle(xBoard, yBoard, "queenside", player))
        {
            Debug.Log("Queenside castling allowed.");
            MovePlateSpawn(xBoard - 2, yBoard);
        }
    }


    public void SetHasMoved(bool moved)
    {
        hasMoved = moved;
    }

    public bool GetHasMoved()
    {
        return hasMoved;
    }

    private void MovePiece(int newX, int newY)
    {
        SetXBoard(newX);
        SetYBoard(newY);
        SetCoords();
        hasMoved = true;
    }

    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            Debug.Log(this.name + " trying to move to: " + x + ", " + y);
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                Debug.Log("Move available at: " + x + ", " + y);
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<Chessman>().player != player)
            {
                Debug.Log("Enemy piece found at: " + x + ", " + y);
                MovePlateAttackSpawn(x, y);
            }
        }
    }


    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);
            }

            if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y);
            }

            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
       
        float x = matrixX;
        float y = matrixY;

        x *= 0.56f;
        y *= 0.56f;

        x += -1.8f;
        y += -2.3f;

        Debug.Log("Instantiating Move Plate at " + x + ", " + y);
        GameObject mp = Instantiate(movePlate, new Vector3(x - 0.15f, y, -1.0f), Quaternion.identity);


        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);

    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.56f;
        y *= 0.56f;

        x += -1.8f;
        y += -2.3f;

        Debug.Log("Instantiating Move Plate at " + x + ", " + y);
        GameObject mp = Instantiate(movePlate, new Vector3(x - 0.15f, y, -1.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    //
    public bool ValidMove(int x, int y)
    {
        if (!controller.GetComponent<Game>().PositionOnBoard(x, y))
        {
            Debug.Log("Invalid move: Outside board bounds");
            return false;
        }

        GameObject target = controller.GetComponent<Game>().GetPosition(x, y);
        if (target == null || target.GetComponent<Chessman>().player != this.player)
        {
            Debug.Log("Valid move to: " + x + ", " + y);
            return true;
        }

        Debug.Log("Invalid move: Occupied by same player");
        return false;
    }

    //

}


