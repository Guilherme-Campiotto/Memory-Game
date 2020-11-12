using System.Collections.Generic;
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

    public static AudioClip menu;
    public static AudioClip theme1;
    public static AudioClip theme2;
    public static AudioClip theme3;
    public static AudioClip theme4;
    public static AudioClip theme5;
    public static AudioClip theme6;
    public static AudioClip themeIntro;
    public static AudioClip themeEnding;
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

        if (!soundObject.activeSelf)
        {
            soundObject.SetActive(true);
        }

        soundController = soundObject.GetComponent<SoundController>();
        CheckMusicTheme(SceneManager.GetActiveScene().buildIndex);

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
    }

    private void CheckMusicTheme(int sceneIndex)
    {
        Debug.Log(sceneIndex);
        switch (sceneIndex)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                if (theme1 == null)
                {
                    themeIntro = theme2 = theme3 = theme4 = theme5 = theme6 = themeEnding = null;
                    theme1= Resources.Load<AudioClip>("Sound/Musics/Adventure_Puzzle_Medieval");
                    soundController.PlayWithLoop(theme1);
                }
                break;
        }
    }

    IEnumerator RevealStars()
    {
        foreach (GameObject objectToReveal in listObjectsToReveal)
        {
            objectToReveal.SetActive(true);
            yield return new WaitForSeconds(timeToShowStars);
        }

        HideButtons();
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
        PlayerPrefs.SetInt("currentLevel", level);
    }

    public int LoadGame()
    {
        int currentLevel = PlayerPrefs.GetInt("currentLevel");
        return currentLevel;
    }
}

