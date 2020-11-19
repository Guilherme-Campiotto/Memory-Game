﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    public int buttonExpected = 0;
    public List<ButtonGame> listButtons;
    public List<ButtonGame> listButtonsComplete;
    public List<GameObject> listObjectsToReveal;

    public Camera mainCamera;
    public bool allowPlayerControl = false;
    public float timeToShowButtons = 0.5f;
    public float timeToShowStars = 0.5f;
    public SoundController soundController;
    public Menu menu;

    AudioClip buttonRightSound;
    AudioClip levelCompleteSound;

    public int currentLevel;

    // Start is called before the first frame update
    public void Start()
    {
        buttonRightSound = Resources.Load<AudioClip>("Sound/Sound_Effects/Button_Click/coin_22");
        levelCompleteSound = Resources.Load<AudioClip>("Sound/Sound_Effects/power_up_18");

        StartCoroutine(ShowButtonsToPlay());

        GameObject soundObject = GameObject.Find("SoundController");
        GameObject menuObject = GameObject.Find("Canvas");

        if (!soundObject.activeSelf)
        {
            soundObject.SetActive(true);
        }

        if (menuObject != null)
        {
            menu = menuObject.GetComponent<Menu>();
        }

        soundController = soundObject.GetComponent<SoundController>();

    }

    // Update is called once per frame
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public bool isButtonCorrect(string name)
    {
        //Debug.Log("Id do botão pressionado: " + bottonId);
        //Debug.Log("Id do botão esperado: " + buttonExpected);
        if(listButtons[buttonExpected].gameObject.name == name)
        {
            if(listButtons.Count - 1 == buttonExpected)
            {
                //Debug.Log("Todos os botões pressionados, proxima fase...");

                soundController.PlayAudioOnce(levelCompleteSound);
                allowPlayerControl = false;
                StartCoroutine(RevealStars());
                return true;
            } else
            {
                //Debug.Log("Botão certo");
                buttonExpected += 1;
                return true;
            }
        } else
        {
            //Debug.Log("Botão errado");
            allowPlayerControl = false;
            menu.ChangeButtonRestartVisibility();

            return false;
        }
    }

    void EndGame()
    {
        Debug.Log("Game ends.");
    }

    public bool isButtonPresssedWrongInsideList(string buttonName)
    {
        bool isInsideList = false;

        foreach(ButtonGame btn in listButtons)
        {
            if(btn.gameObject.name.Equals(buttonName))
            {
                isInsideList = true;
            }
        }

        return isInsideList;
    }

    public void NextLevel()
    {

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        
        SaveGame(nextScene);

        if (nextScene > 11)
        {
            EndGame();
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    IEnumerator ShowButtonsToPlay()
    {
        yield return new WaitForSeconds(timeToShowButtons);
        foreach (ButtonGame button in listButtons)
        {
            //Debug.Log("Acendeu");
            button.GetComponent<SpriteRenderer>().sprite = button.buttonOnSprite;
            soundController.PlayAudioOnce(buttonRightSound);
            yield return new WaitForSeconds(timeToShowButtons);
            //Debug.Log("Apagou");
            button.GetComponent<SpriteRenderer>().sprite = button.buttonOffSprite;
        }

        allowPlayerControl = true;
    }

    public void ResetButtons()
    {
        foreach(ButtonGame button in listButtonsComplete) {
            button.GetComponent<SpriteRenderer>().sprite = button.buttonOffSprite;
            button.isClickable = true;
        }

        buttonExpected = 0;

        StartCoroutine(ShowButtonsToPlay());

        menu.ChangeButtonRestartVisibility();
    }

    IEnumerator RevealStars()
    {
        foreach (GameObject objectToReveal in listObjectsToReveal)
        {
            objectToReveal.SetActive(true);
            yield return new WaitForSeconds(timeToShowStars);
        }

        HideButtons();

        menu.ChangeButtonNextVisibility();
    }

    private void HideButtons()
    {
        foreach (ButtonGame button in listButtonsComplete)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void SaveGame(int level)
    {
        PlayerPrefs.SetInt("PlayerProgress", level);
    }

}

