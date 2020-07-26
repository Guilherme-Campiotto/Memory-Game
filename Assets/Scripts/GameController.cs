using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    public int buttonExpected = 0;
    public List<ButtonGame> listButtons;
    public List<ButtonGame> listButtonsComplete;
    public Camera mainCamera;
    public bool allowPlayerControl = false;
    public float timeToShowButtons = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowButtonsToPlay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool isButtonCorrect(int bottonId)
    {
        Debug.Log("Id do botão pressionado: " + bottonId);
        Debug.Log("Id do botão esperado: " + buttonExpected);
        if(listButtons[buttonExpected].id == bottonId)
        {
            if(listButtons.Count - 1 == buttonExpected)
            {
                Debug.Log("Todos os botões pressionados, proxima fase...");
                allowPlayerControl = false;
                return true;
            } else
            {
                Debug.Log("Botão certo");
                buttonExpected += 1;
                return true;
            }
        } else
        {
            Debug.Log("Botão errado");
            allowPlayerControl = false;
            return false;
        }
    }

    void EndGame()
    {
        Debug.Log("Game ends.");
    }

    public void NextLevel()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextScene > 5)
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
            Debug.Log("Acendeu");
            button.GetComponent<SpriteRenderer>().sprite = button.buttonOnSprite;
            yield return new WaitForSeconds(timeToShowButtons);
            Debug.Log("Apagou");
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
    }
}

