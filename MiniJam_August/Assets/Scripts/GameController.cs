using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

        titleCanvas.gameObject.SetActive(true);
        placingPieces = false;
    }


    void Update()
    {
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
        Debug.Log("Playing the game");

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
}
