using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGame : MonoBehaviour
{
    public int id;
    public bool isButtonInSequence;
    public bool isClickable = true;
    
    public bool isMovingX = false;
    public bool isMovingY = false;
    public float xPosition;
    public float spaceToMove;
    public int yPosition;
    public float speedMovement = 5;

    public Sprite buttonOnSprite;
    public Sprite buttonOffSprite;
    public Sprite buttonWrongSprite;

    AudioClip buttonRightSound;
    AudioClip buttonWrongSound;

    SoundController soundController;
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        buttonRightSound = Resources.Load<AudioClip>("Sound/Sound_Effects/Button_Click/coin_22");
        buttonWrongSound = Resources.Load<AudioClip>("Sound/Sound_Effects/Button_Click/game_over_20");

        gameController = GameObject.Find("GameController").GetComponent<GameController>();

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
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && isClickable && gameController.allowPlayerControl) {
            bool isButtonCorrect = gameController.isButtonCorrect(id);
            isClickable = false;
            if (isButtonCorrect)
            {
                soundController.PlayAudioOnce(buttonRightSound);
                GetComponent<SpriteRenderer>().sprite = buttonOnSprite;

            } else
            {
                soundController.PlayAudioOnce(buttonWrongSound);
                GetComponent<SpriteRenderer>().sprite = buttonWrongSprite;
            }
        }
    }
}
