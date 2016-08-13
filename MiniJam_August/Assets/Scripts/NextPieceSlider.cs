using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NextPieceSlider : MonoBehaviour
{
    Slider slider;
    Image image;

    void Awake()
    {
        slider = GetComponent<Slider>();
        image = GetComponentsInChildren<Image>()[1];
    }

    void Update()
    {
        //the slider value should be the same percentage filled as time since last piece
        //if time for a new piece, slider will change colour


        slider.value = GameController.controller.percent;

        if (GameController.controller.percent >= 1 && GridHighlighter.controller.hasPiece)
        {

            StartCoroutine(Change());
        }
        else
        {
            image.color = Color.white;
        }

    }

    IEnumerator Change()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        image.color = Color.red;

        yield return null;
    }

    public void ChangeColor(Color c)
    {
        image.color = c;
    }
}
