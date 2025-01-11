using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate2 : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    // Board positions not world position
    int matrixX;
    int matrixY;

    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            //Set to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        //Destroy the victim Chesspiece
        if (attack)
        {
            GameObject cp = controller.GetComponent<Game2>().GetPosition(matrixX, matrixY);
            Destroy(cp);
        }

        //Set the Chesspiece's original location to be empty
        controller.GetComponent<Game2>().SetPositionEmpty(reference.GetComponent<Chessman2>().GetXBoard(),
            reference.GetComponent<Chessman2>().GetYBoard());

        //Move reference chess piece to this position
        reference.GetComponent<Chessman2>().SetXBoard(matrixX);
        reference.GetComponent<Chessman2>().SetYBoard(matrixY);
        reference.GetComponent<Chessman2>().SetCoords();

        //Update the matrix
        controller.GetComponent<Game2>().SetPosition(reference);

        //Destroy the move plates including self
        reference.GetComponent<Chessman2>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }

}
