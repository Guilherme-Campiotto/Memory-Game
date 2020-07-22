using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGame : MonoBehaviour
{
    public int id;
    public bool isButtonInSequence;
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
        if (Input.GetMouseButtonDown(0)) {
            gameController.CheckOrderButtonsPressed(id);
        }
    }
}
