using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

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
    public bool isInvisible = false;
    public bool canTeleport = false;

    public GameObject destiny;
    public AudioClip teleportClip;

    private Vector3 inicialPosition;
    public float inicialSpeedMovement;
    public float inicialLightIntensity;

    private float teleportSpeed = 1f;
    
    Animator animator;

    AudioClip buttonRightSound;
    AudioClip buttonWrongSound;

    SoundController soundController;
    GameController gameController;
    DebugMode debugMode;

    void Start()
    {
        inicialPosition = transform.position;
        inicialSpeedMovement = speedMovement;
        inicialLightIntensity = GetComponentInChildren<Light2D>().intensity;
        buttonRightSound = Resources.Load<AudioClip>("Sound/Sound_Effects/Button_Click/coin_22");
        buttonWrongSound = Resources.Load<AudioClip>("Sound/Sound_Effects/Button_Click/game_over_20");
        teleportClip = Resources.Load<AudioClip>("Sound/Sound_Effects/teleport1");
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
                    animator.SetBool("Invisible", false);
                    animator.SetBool("ButtonOff", false);
                    animator.SetBool("ButtonWrong", true);
                }
                else if (gameController.isButtonPresssedWrongInsideList(gameObject.name))
                {
                    animator.SetBool("Invisible", false);
                    animator.SetBool("ButtonOff", false);
                    animator.SetBool("ButtonWrongOrder", true);
                } else
                {
                    animator.SetBool("Invisible", false);
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
        speedMovement = inicialSpeedMovement;
        transform.position = inicialPosition;
        yPosition = 0;
        xPosition = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    public IEnumerator Teleport(Animator animator)
    {
        soundController.PlayAudioOnce(teleportClip);
        animator.SetBool("Teleporting", true);
        animator.SetBool("ButtonOff", false);

        destiny.GetComponent<Animator>().SetBool("Teleporting", true);

        yield return new WaitForSeconds(teleportSpeed);

        animator.SetBool("Teleporting", false);
        animator.SetBool("ButtonOff", true);
        destiny.GetComponent<Animator>().SetBool("Teleporting", false);

        transform.position = destiny.GetComponent<Transform>().position;
    }

    public void IncreaseLightButtonPressed()
    {
        GetComponentInChildren<Light2D>().intensity = 2f;
    }

    public void NormalizeLightButtonOff()
    {
        GetComponentInChildren<Light2D>().intensity = inicialLightIntensity;
    }

}
