

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate_L : MonoBehaviour
{
    
    public GameObject controller;

    GameObject reference = null;

    
    int matrixX;
    int matrixY;

   
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
           
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (attack)
        {
            GameObject cp = controller.GetComponent<Game_L>().GetPosition(matrixX, matrixY);

            if (cp.name == "white_king") controller.GetComponent<Game_L>().Winner("black");
            if (cp.name == "black_king") controller.GetComponent<Game_L>().Winner("white");

            Destroy(cp);
        }

        controller.GetComponent<Game_L>().SetPositionEmpty(reference.GetComponent<Chessman_L>().GetXBoard(),
            reference.GetComponent<Chessman_L>().GetYBoard());

      
        reference.GetComponent<Chessman_L>().SetXBoard(matrixX);
        reference.GetComponent<Chessman_L>().SetYBoard(matrixY);
        reference.GetComponent<Chessman_L>().SetCoords();

      
        controller.GetComponent<Game_L>().SetPosition(reference);

      
        controller.GetComponent<Game_L>().NextTurn();

       
        reference.GetComponent<Chessman_L>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public int GetX()
    {
        return matrixX;
    }

    public int GetY()
    {
        return matrixY;
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


