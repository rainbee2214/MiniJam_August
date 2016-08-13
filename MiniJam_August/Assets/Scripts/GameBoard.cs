using UnityEngine;
using System.Collections;

public class GameBoard : MonoBehaviour
{
    public bool canI = false;

    //This doesn't appear to be working properly? I dunno what's going on
    //void OnMouseOver()
    //{
    //    Debug.Log("Mouse over");
    //    canI = true;
    //    GameController.controller.canIEvenPlace = true;
    //}
    //void OnMouseExit()
    //{
    //    Debug.Log("Mouse exit");

    //    canI = false;
    //    GameController.controller.canIEvenPlace = false;
    //}


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Piece")
        {
            GameController.controller.canPlace = true;
            Debug.Log("Something is touching the game board " + other.tag);
            //show the piece
            GridHighlighter.controller.sr.enabled = true;
            GameController.controller.currentPiece.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Piece")
        {
            GameController.controller.canPlace = false;
            GridHighlighter.controller.sr.enabled = false;
            GameController.controller.currentPiece.GetComponent<SpriteRenderer>().enabled = false;

        }
    }
}
