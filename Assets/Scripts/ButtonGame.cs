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
    public bool fakeButton = false;

    public Vector3 inicialPosition;

    Animator animator;

    AudioClip buttonRightSound;
    AudioClip buttonWrongSound;

    SoundController soundController;
    GameController gameController;
    DebugMode debugMode;

    void Start()
    {
        inicialPosition = transform.position;
        buttonRightSound = Resources.Load<AudioClip>("Sound/Sound_Effects/Button_Click/coin_22");
        buttonWrongSound = Resources.Load<AudioClip>("Sound/Sound_Effects/Button_Click/game_over_20");
        animator = GetComponent<Animator>();

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        debugMode = GameObject.Find("Canvas").GetComponent<DebugMode>();

        GameObject soundObject = GameObject.Find("SoundController");

        if (!soundObject.activeSelf)
        {
            soundObject.SetActive(true);
        }

        soundController = soundObject.GetComponent<SoundController>();
    }

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
                animator.SetBool("ButtonOn", true);
                animator.SetBool("ButtonOff", false);

            } else
            {
                if(fakeButton == true)
                {
                    animator.SetBool("ButtonOff", false);
                    animator.SetBool("ButtonWrong", true);
                }
                else if (gameController.isButtonPresssedWrongInsideList(gameObject.name))
                {
                    animator.SetBool("ButtonOff", false);
                    animator.SetBool("ButtonWrongOrder", true);
                } else
                {
                    animator.SetBool("ButtonOff", false);
                    animator.SetBool("ButtonWrong", true);
                }

                soundController.PlayAudioOnce(buttonWrongSound);
                
                if(debugMode != null)
                {
                    debugMode.numberOfTries++;
                }

            }
        }
    }

    public void ResetPosition()
    {
        transform.position = inicialPosition;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
}
