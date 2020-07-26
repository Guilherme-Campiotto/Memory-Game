using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGame : MonoBehaviour
{
    public int id;
    public bool isButtonInSequence;
    public bool isClickable = true;
    public Sprite buttonOnSprite;
    public Sprite buttonOffSprite;
    public Sprite buttonWrongSprite;

    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && isClickable && gameController.allowPlayerControl) {
            bool isButtonCorrect = gameController.isButtonCorrect(id);
            isClickable = false;
            if (isButtonCorrect)
            {
                GetComponent<SpriteRenderer>().sprite = buttonOnSprite;
            } else
            {
                GetComponent<SpriteRenderer>().sprite = buttonWrongSprite;
            }
        }
    }
}
