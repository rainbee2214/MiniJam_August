using UnityEngine;
using System.Collections;

public class GameBoard : MonoBehaviour
{

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Piece")
        {
            GameController.controller.canPlace = true;
            //show the piece
            GridHighlighter.controller.sr.enabled = true;
            if (GameController.controller.currentPiece != null) GameController.controller.currentPiece.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Piece")
        {
            GameController.controller.canPlace = false;
            GridHighlighter.controller.sr.enabled = false;
            if (GameController.controller.currentPiece != null) GameController.controller.currentPiece.GetComponent<SpriteRenderer>().enabled = false;

        }
    }
}
