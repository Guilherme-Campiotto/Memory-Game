using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGame : MonoBehaviour
{
    public bool isButtonInSequence;
    public bool isClickable = true;
    
    public bool isMovingX = false;
    public bool isMovingY = false;
    public float xPosition;
    public float spaceToMove;
    public float yPosition;
    public float speedMovement = 5;

    public Sprite buttonOnSprite;
    public Sprite buttonOffSprite;
    public Sprite buttonWrongRedSprite;
    public Sprite buttonWrongYellowSprite;

    AudioClip buttonRightSound;
    AudioClip buttonWrongSound;

    SoundController soundController;
    GameController gameController;
    DebugMode debugMode;


    // Start is called before the first frame update
    void Start()
    {
        buttonRightSound = Resources.Load<AudioClip>("Sound/Sound_Effects/Button_Click/coin_22");
        buttonWrongSound = Resources.Load<AudioClip>("Sound/Sound_Effects/Button_Click/game_over_20");

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        debugMode = GameObject.Find("Canvas").GetComponent<DebugMode>();

        GameObject soundObject = GameObject.Find("SoundController");

        if (!soundObject.activeSelf)
        {
            soundObject.SetActive(true);
        }

        soundController = soundObject.GetComponent<SoundController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMovingX)
        {
            Vector3 targetVelocity = new Vector2(transform.position.x + (speedMovement * Time.fixedDeltaTime), transform.position.y);
            transform.position = targetVelocity;
            xPosition += Mathf.Abs(speedMovement * Time.fixedDeltaTime);

            if(xPosition >= spaceToMove)
            {
                xPosition = 0;
                speedMovement = speedMovement * -1;
            }
        }

        if(isMovingY)
        {
            Vector3 targetVelocity = new Vector2(transform.position.x, transform.position.y + (speedMovement * Time.fixedDeltaTime));
            transform.position = targetVelocity;
            yPosition += Mathf.Abs(speedMovement * Time.fixedDeltaTime);

            if (yPosition >= spaceToMove)
            {
                yPosition = 0;
                speedMovement = speedMovement * -1;
            }
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && isClickable && gameController.allowPlayerControl) {
            bool isButtonCorrect = gameController.isButtonCorrect(gameObject.name);
            isClickable = false;
            if (isButtonCorrect)
            {
                soundController.PlayAudioOnce(buttonRightSound);
                GetComponent<SpriteRenderer>().sprite = buttonOnSprite;

            } else
            {
                if(gameController.isButtonPresssedWrongInsideList(gameObject.name))
                {
                    GetComponent<SpriteRenderer>().sprite = buttonWrongYellowSprite;
                } else
                {
                    GetComponent<SpriteRenderer>().sprite = buttonWrongRedSprite;
                }

                soundController.PlayAudioOnce(buttonWrongSound);
                
                if(debugMode != null)
                {
                    debugMode.numberOfTries++;
                }

            }
        }
    }
}
