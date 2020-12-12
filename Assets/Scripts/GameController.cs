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
    public List<ButtonGame> buttonsFiltered;

    public Camera mainCamera;
    public bool allowPlayerControl = false;
    public float timeToShowButtons = 0.5f;
    public float timeToShowStars = 0.5f;
    public SoundController soundController;
    public Menu menu;

    AudioClip buttonRightSound;
    AudioClip levelCompleteSound;
    AudioClip starsRevealed;

    public int currentLevel;
    private GameObject steamAchievements;
    SteamAchievements scriptAchievments;

    public void Start()
    {
        //QualitySettings.vSyncCount = 0;  // VSync must be disabled
        //Application.targetFrameRate = 200; // test

        buttonRightSound = Resources.Load<AudioClip>("Sound/Sound_Effects/Button_Click/coin_22");
        levelCompleteSound = Resources.Load<AudioClip>("Sound/Sound_Effects/power_up_18");
        starsRevealed = Resources.Load<AudioClip>("Sound/Sound_Effects/star_3");

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

        steamAchievements = GameObject.Find("SteamAchievements");

        if (steamAchievements != null)
        {
            scriptAchievments = steamAchievements.GetComponent<SteamAchievements>();
        }

        soundController.LoadGameVolume();

    }

    public bool isButtonCorrect(string name)
    {
        //Debug.Log("Id do botão pressionado: " + bottonId);
        //Debug.Log("Id do botão esperado: " + buttonExpected);
        if(buttonsFiltered[buttonExpected].gameObject.name == name)
        {
            if(buttonsFiltered.Count - 1 == buttonExpected)
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

        foreach(ButtonGame btn in buttonsFiltered)
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

        if (scriptAchievments != null)
        {
            switch(SceneManager.GetActiveScene().buildIndex)
            {
                case 6:
                    Debug.Log("Achievement 1/0: ");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_0");
                    break;
                case 11:
                    Debug.Log("Achievement 1/1: Zerar o jogo");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_1");
                    break;
                case 16:
                    Debug.Log("Achievement 1/2: Zerar o jogo");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_2");
                    break;
                case 21:
                    Debug.Log("Achievement 1/3: Zerar o jogo");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_3");
                    break;
                case 26:
                    Debug.Log("Achievement 1/4: Zerar o jogo");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_4");
                    break;
                case 31:
                    Debug.Log("Achievement 1/5: Zerar o jogo");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_5");
                    break;
                case 36:
                    Debug.Log("Achievement 1/6: Zerar o jogo");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_6");
                    break;
                case 41:
                    Debug.Log("Achievement 1/7: Zerar o jogo");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_7");
                    break;
                case 46:
                    Debug.Log("Achievement 1/8: Zerar o jogo");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_8");
                    break;
                case 51:
                    Debug.Log("Achievement 1/9: Zerar o jogo");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_9");
                    break;
                case 56:
                    Debug.Log("Achievement 1/10: Zerar o jogo");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_10");
                    break;
                case 61:
                    Debug.Log("Achievement 1/11: Zerar o jogo");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_11");
                    break;
            }

        }

        if (nextScene > 62)
        {
            EndGame();
        }
        else
        {
            SaveGame(nextScene);
            SceneManager.LoadScene(nextScene);
        }
    }

    IEnumerator ShowButtonsToPlay()
    {
        Animator animatorBtn;
        yield return new WaitForSeconds(timeToShowButtons);
        foreach (ButtonGame button in listButtons)
        {
            animatorBtn = button.GetComponent<Animator>();

            if(button.fakeButton)
            {
                animatorBtn.SetBool("ButtonOff", false);
                animatorBtn.SetBool("ButtonWrong", true);

                soundController.PlayAudioOnce(buttonRightSound);
                yield return new WaitForSeconds(timeToShowButtons);
                animatorBtn.SetBool("ButtonWrong", false);
                animatorBtn.SetBool("ButtonOff", true);
            } else
            {
                animatorBtn.SetBool("ButtonOn", true);
                animatorBtn.SetBool("ButtonOff", false);

                soundController.PlayAudioOnce(buttonRightSound);
                yield return new WaitForSeconds(timeToShowButtons);
                animatorBtn.SetBool("ButtonOn", false);
                animatorBtn.SetBool("ButtonOff", true);
                button.NormalizeLightButtonOff();
            }

        }

        RemoveFakeButtons();
        StartCoroutine(HideInvisibleButtons());
        StartCoroutine(TeleportButtons());
    }

    IEnumerator HideInvisibleButtons()
    {
        Animator animatorBtn;

        foreach (ButtonGame button in listButtonsComplete)
        {
            animatorBtn = button.GetComponent<Animator>();
            if (button.isInvisible)
            {
                animatorBtn.SetBool("Invisible", true);
                animatorBtn.SetBool("ButtonOff", false);
                yield return new WaitForSeconds(0.3f);
            }

        }

    }

    IEnumerator TeleportButtons()
    {
        Animator animatorBtn;
        yield return new WaitForSeconds(0.3f);
        foreach (ButtonGame button in listButtonsComplete)
        {
            animatorBtn = button.GetComponent<Animator>();
            if (button.canTeleport)
            {
                StartCoroutine(button.Teleport(animatorBtn));
                yield return new WaitForSeconds(1f);
            }
        }

        allowPlayerControl = true;
    }

    public void ResetButtons()
    {
        Animator animatorBtn;
        foreach (ButtonGame button in listButtonsComplete) {
            animatorBtn = button.GetComponent<Animator>();
            animatorBtn.SetBool("ButtonOn", false);
            animatorBtn.SetBool("ButtonOff", true);
            animatorBtn.SetBool("ButtonWrong", false);
            animatorBtn.SetBool("ButtonWrongOrder", false);
            animatorBtn.SetBool("Invisible", false);
            button.isClickable = true;
            button.ResetPosition();
            button.NormalizeLightButtonOff();

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

        soundController.PlayAudioOnce(starsRevealed, 0.8f);
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
        int levelSaved = PlayerPrefs.GetInt("PlayerProgress");

        if(levelSaved < level)
        {
            PlayerPrefs.SetInt("PlayerProgress", level);
        }
    }

    void RemoveFakeButtons()
    {
        buttonsFiltered = new List<ButtonGame>();
        foreach (ButtonGame button in listButtons)
        {
            if (!button.fakeButton)
            {
                buttonsFiltered.Add(button);
            }
        }
    }

}