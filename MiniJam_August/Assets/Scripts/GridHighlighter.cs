using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridHighlighter : MonoBehaviour
{
    public static GridHighlighter controller;
    public bool hasPiece;

    public SpriteRenderer sr;
    public bool turnOn;

    public GameObject piecePrefab;

    public List<GameObject> pieces;
    public List<Color> colours;

    void Awake()
    {
        controller = this;
        sr = GetComponent<SpriteRenderer>();
        pieces = new List<GameObject>();

        if (colours == null)
        {
            colours = new List<Color> { Color.white, Color.black, Color.blue, Color.green, Color.red, Color.yellow };
        }
    }

    void Update()
    {
        //if (!GameController.controller.canIEvenPlace)
        //{
        //    return;
        //}
        if (hasPiece)
        {
            if (turnOn)
            {
                turnOn = false;
                sr.enabled = true;
            }
            transform.position = Piece.GetClosest(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else
        {
            sr.enabled = false;
            turnOn = true;
        }

    }

    public void AddPiece(int colorIndex)
    {
        //check if there are any turned off in the current list
        //if not add one
        bool add = true;
        foreach (GameObject g in pieces)
        {
            if (!g.activeInHierarchy)
            {
                g.GetComponent<Piece>().Setup(true, colours[colorIndex], colorIndex);
                //Debug.Log("Found an inactive piece ");
                g.GetComponent<Piece>().Setup(true, colours[colorIndex], colorIndex);
                g.gameObject.SetActive(true);
                hasPiece = true;
                GameController.controller.currentPiece = g;
                add = false;
                break;
            }
        }

        if (add)
        {
            Debug.Log("Haven't found an inactive piece");
            pieces.Add(Instantiate(piecePrefab));
            pieces[pieces.Count - 1].GetComponent<Piece>().Setup(true, colours[colorIndex], colorIndex);
            GameController.controller.currentPiece = pieces[pieces.Count - 1];
            hasPiece = true;
        }
    }
}
