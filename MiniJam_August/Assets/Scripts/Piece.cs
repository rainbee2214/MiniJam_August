using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Piece : MonoBehaviour
{
    float deltaY = 0.84f;
    float deltaX = 1f;
    public bool followingMouse = false;

    SpriteRenderer sr;

    public int currentColor;

    public bool placed = false;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public void OnMouseDown()
    {
        if (!GameController.controller.canPlace || placed) return;
        if (GameController.controller.placingPieces)
        {
            Pickup();
        }
        else
        {

        }
    }

    public void OnMouseUp()
    {
        if (!GameController.controller.canPlace) return;
        if (GameController.controller.placingPieces)
        {
            Place(transform.position);
        }
        else
        {
            //Place a piece that destroys everything it touches and possible scores points
            Debug.Log("Trying to destroy blocks!");
            //Place(transform.position);
            GameController.controller.DestroyPieces(currentColor);
            AudioController.controller.PlaySound(SoundType.BadMove);
            gameObject.SetActive(false);
            GridHighlighter.controller.hasPiece = false;

        }
    }


    public void Setup(bool followMouse, Color color, int colorIndex)
    {
        tag = "Piece";
        placed = false;
        //turn on the piece
        followingMouse = followMouse;
        sr.color = color;
        currentColor = colorIndex;
    }

    void Update()
    {
        if (followingMouse)
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public void Pickup()
    {
        followingMouse = true;
        GridHighlighter.controller.hasPiece = true;
        placed = true;
    }

    public void Place(Vector2 position)
    {

        tag = "Untagged";
        GameController.controller.currentPiece = null;
        AudioController.controller.PlaySound(SoundType.Place);
        //if (!GameController.controller.canIEvenPlace) return;
        followingMouse = false;
        GridHighlighter.controller.hasPiece = false;
        transform.position = GetClosest(position, deltaY);

        //if we've placed and there is a built up piece ready, give it to me now
        if (GameController.controller.percent >= 1)
        {
            GameController.controller.OverrideSendPiece();
        }
        GameController.controller.percent = 0;
    }

    public static Vector2 GetClosest(Vector2 position, float deltaY = 0.84f)
    {
        //Needs to be in positive region
        //Given position, return a modified position that is snapped to the isometric grid
        float topY = 0f, bottomY = 0f; //get the nearest positions in each direction -> choose the closest
        //If I have a y value

        int y = (int)Mathf.RoundToInt(position.y / deltaY);
        bottomY = deltaY * y;
        topY = deltaY * (y + 1);

        float x1 = 0, y1 = 0;

        y1 = (Mathf.Abs(topY - y1) < Mathf.Abs(bottomY - y1)) ? topY : bottomY;
        //x1 = ((Mathf.Abs(rightX - x1) < Mathf.Abs(leftX - x1)) ? rightX : leftX) + ((y % 2 == 0) ? 0 : 0.5f);

        if (y % 2 == 0) x1 = Mathf.RoundToInt(position.x);// +((y % 2 == 0) ? 0 : 0.5f);
        else
        {
            float xx = Mathf.Floor(position.x) + 0.5f;
            float xy = Mathf.Ceil(position.x) + 0.5f;
            //x must be closer to either to the floor + 0.5 or ceil + 0.5
            x1 = (Mathf.Abs(xx - x1) < Mathf.Abs(xy - x1)) ? xx : xy;
        }
        //Debug.Log("x " + position.x + " rounded " + Mathf.RoundToInt(position.x));
        return new Vector2(x1, y1);
    }
}
