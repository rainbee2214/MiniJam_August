using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Match
{
    public Match(Vector2 location, Piece piece, int color)
    {
        this.location = location;
        this.piece = piece;
        this.color = color;
    }
    public Vector2 location;
    public Piece piece;
    public int color;


}
public class GameController : MonoBehaviour
{
    public static GameController controller;
    public GameObject titleCanvas;

    public bool canIEvenPlace = false;
    public bool placingPieces = true;

    public bool playingGame = false;
    public bool startGame = false;

    public Image nextPieceImage;
    public float percent = 0;

    public GameObject currentPiece;
    public bool canPlace = false;

    List<List<Match>> matchesSoFar;
    void Awake()
    {
        if (controller == null)
        {
            DontDestroyOnLoad(controller);
            controller = this;
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }

        matchesSoFar = new List<List<Match>>();
        titleCanvas.gameObject.SetActive(true);
        titleCanvas.gameObject.SetActive(true);
        placingPieces = false;
    }


    void Update()
    {
        Debug.Log("Number of lists " + matchesSoFar.Count);
        percent = (Time.time - GameController.controller.lastSendTime) / GameController.controller.spawnPieceDelay;

        if (Input.GetButtonDown("StartGame") && startGame)
        {
            startGame = false;
            StartCoroutine(PlayGame());
        }

        if (GridHighlighter.controller.hasPiece && Input.GetMouseButtonDown(1))
        {
            //remove the current piece
            GridHighlighter.controller.hasPiece = false;
            GameController.controller.currentPiece.SetActive(false);
            GameController.controller.currentPiece = null;
            AudioController.controller.PlaySound(SoundType.Create);
        }
    }

    IEnumerator PlayGame()
    {
        titleCanvas.gameObject.SetActive(false);

        playingGame = true;

        colourIndex = Random.Range(0, GridHighlighter.controller.colours.Count);
        StartCoroutine(StartSwitchingModes());
        StartCoroutine("SendPieces");

        while (playingGame)
        {
            yield return null;
        }
        //Start the game

        //Show a game board for the current level

        //

        yield return null;
    }

    public float spawnPieceDelay = 1f;
    public int colourIndex;

    public float lastSendTime;

    IEnumerator SendPieces()
    {
        ShowNextPiece();
        while (playingGame)
        {
            if (!GridHighlighter.controller.hasPiece)
            {
                SendPiece();
            }
            //now the next colour can be displayed
            yield return new WaitForSeconds(spawnPieceDelay);
        }
        yield return null;
    }

    void SendPiece()
    {
        GridHighlighter.controller.AddPiece(colourIndex);
        lastSendTime = Time.time;
        colourIndex = Random.Range(0, GridHighlighter.controller.colours.Count);
        ShowNextPiece();
    }

    public float lastSwitchTime;

    public float placingTime = 5f, destructionTime = 2f;
    IEnumerator StartSwitchingModes()
    {
        while (playingGame)
        {
            if (placingPieces)
            {
                placingPieces = false;
                yield return new WaitForSeconds(destructionTime);
            }
            else
            {
                placingPieces = true;
                yield return new WaitForSeconds(placingTime);
            }
            yield return null;
            lastSwitchTime = Time.time;
        }
        yield return null;
    }

    public void OverrideSendPiece()
    {
        StartCoroutine(OverrideSend());
    }

    IEnumerator OverrideSend()
    {
        StopCoroutine("SendPieces");
        StartCoroutine("SendPieces");
        //this is to send a peice right after clicking on it
        yield return null;
    }

    public void ShowNextPiece()
    {
        nextPieceImage.color = GridHighlighter.controller.colours[colourIndex];
    }

    public void DestroyPieces(Vector2 position)
    {
        //destory the list of pieces with this match
        Debug.Log("Destroying a match " + position);
        bool found = false;
        List<Match> listToDestroy = null;

        //check each list, and each piece  in the list to see if the current piece is touching and should be added to a list or a new list
        foreach (List<Match> l in matchesSoFar)
        {
            foreach (Match m in l)
            {
                //
                if (Vector2.Distance(m.location, position) < 0.5f) listToDestroy = l;
            }
        }

        if (listToDestroy != null)
        {
            StartCoroutine(Destroy(listToDestroy));
        }
    }

    IEnumerator Destroy(List<Match> listToDestroy)
    {
        int k = listToDestroy.Count;
        Debug.Log("Destroying " + k + " pieces!");
        for (int i = 0; i < k; i++)
        {
            Debug.Log("Destroying " + listToDestroy[0]);
            listToDestroy[0].piece.gameObject.SetActive(false);
            listToDestroy.RemoveAt(0);
            yield return null;
        }
        matchesSoFar.Remove(listToDestroy);
        yield return null;
    }

    public void DestroyPieces(int color)
    {
        int i = GridHighlighter.controller.pieces.Count; // this is how many times we need to look
        int k = 0; //offset for how far in we need to look, this is to combat changing the size of the list while deleting things :)

        for (int j = 0; j < i; j++)
        {
            if (GridHighlighter.controller.pieces[k].GetComponent<Piece>().currentColor != color)
            {
                k++; //increase the offset by 1 and leave it
            }
            else
            {
                if (GridHighlighter.controller.pieces[k].GetComponent<Piece>().placed)
                {
                    GridHighlighter.controller.pieces[k].SetActive(false);
                    GridHighlighter.controller.pieces.RemoveAt(k);
                }
            }
        }
    }

    public void AddToMatches(Match newMatch)
    {
        Debug.Log("Adding a match");
        bool found = false;
        //check each list, and each piece  in the list to see if the current piece is touching and should be added to a list or a new list
        foreach (List<Match> l in matchesSoFar)
        {
            foreach (Match m in l)
            {
                //
                if (Touching(newMatch.location, m.location) && SameColor(newMatch.color, m.color))
                {
                    Debug.Log("Adding to list of matches");
                    l.Add(newMatch);
                    found = true;
                    break;
                }
            }
            if (found) break;
        }

        if (!found)
        {
            //check for an empty list to reuse
            foreach (List<Match> l in matchesSoFar)
            {
                if (l.Count == 0)
                {
                    l.Add(newMatch);
                    found = true;
                    Debug.Log("Reusing list");
                }

            }
            if (!found)
            {
                Debug.Log("New list of matches");
                matchesSoFar.Add(new List<Match>() { newMatch });
            }
        }
    }

    bool Touching(Vector2 p1, Vector2 p2)
    {
        //two pieces are touching if: they are touchng left or right, top left or right, bottom left or right
        //deltaX, deltaY
        Debug.Log((Mathf.Abs(p1.x - p2.x) <= 1f) && (Mathf.Abs(p1.y - p2.y) < 0.85f));
        return (Mathf.Abs(p1.x-p2.x) <= 1f) && (Mathf.Abs(p1.y-p2.y) < 0.85f);
    }

    public static bool SameColor(int c1, int c2)
    {
        return c1 == c2;
    }

    public static bool MtachEquals(Match m1, Match m2)
    {
        return (m2.location == m1.location) && (m2.piece == m1.piece) && (m2.color == m1.color);
    }

    public bool HasPiece(Vector2 location)
    {
        //return false;
        foreach (GameObject p in GridHighlighter.controller.pieces)
        {
            if (!p.GetComponent<Piece>().placed || !p.activeInHierarchy) continue;
            if ((Vector2)p.transform.position== location)
            {
                return true;
            }
        }
        return false;
    }
}
