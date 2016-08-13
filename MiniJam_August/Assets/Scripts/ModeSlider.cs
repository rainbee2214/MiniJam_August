using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ModeSlider : MonoBehaviour
{
    Slider slider;
    public Color placingColor, destructionColor;

    public Image image;
    public Text text;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        //Move slider value back and forth between modes -> placing mode and destruction mode

        if (GameController.controller.placingPieces)
        {
            //going towards destruction mode
            slider.value = (Time.time - GameController.controller.lastSwitchTime) / GameController.controller.placingTime;
            image.color = placingColor;
            text.text = "Place Blocks";
        }
        else
        {
            //going towards placing mode
            slider.value = (1 - Mathf.Min(1, (Time.time - GameController.controller.lastSwitchTime) / GameController.controller.destructionTime));
            image.color = destructionColor;
            text.text = "Destroy Blocks";
        }
    }
}
