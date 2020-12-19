using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Experimental.Rendering.LWRP;

public class GameController : MonoBehaviour
{
    public int buttonExpected = 0;
    public List<ButtonGame> listButtons;
    public List<ButtonGame> listButtonsComplete;
    public List<GameObject> listObjectsToReveal;
    public List<ButtonGame> buttonsFiltered;
    public List<ButtonGame> buttonsInvisible;
    public List<ButtonGame> buttonsTeleport;

    public Camera mainCamera;
    public bool allowPlayerControl = false;
    public float timeToShowButtons = 0.5f;
    public float timeToShowStars = 0.5f;
    public SoundController soundController;
    public Menu menu;

    AudioClip buttonRightSound;
    AudioClip levelCompleteSound;
    AudioClip starsRevealed;

    public List<AudioClip> starSounds;

    public int currentLevel;
    private GameObject steamAchievements;
    SteamAchievements scriptAchievments;

    GameObject animationWonPrefab;

    public void Start()
    {
        //QualitySettings.vSyncCount = 0;  // VSync must be disabled
        //Application.targetFrameRate = 200; // test

        buttonRightSound = Resources.Load<AudioClip>("Sound/Sound_Effects/Button_Click/coin_22");
        levelCompleteSound = Resources.Load<AudioClip>("Sound/Sound_Effects/power_up_18");
        starsRevealed = Resources.Load<AudioClip>("Sound/Sound_Effects/star_3");

        starSounds = new List<AudioClip>
        {
            Resources.Load<AudioClip>("Sound/Sound_Effects/star_1"),
            Resources.Load<AudioClip>("Sound/Sound_Effects/star_2")
        };

        animationWonPrefab = Resources.Load<GameObject>("Prefabs/GameWonEffect");
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
        getButtonsInvisible();
        getButtonsTeleport();

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
                    Debug.Log("Achievement 1/1");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_1");
                    break;
                case 16:
                    Debug.Log("Achievement 1/2");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_2");
                    break;
                case 21:
                    Debug.Log("Achievement 1/3");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_3");
                    break;
                case 26:
                    Debug.Log("Achievement 1/4");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_4");
                    break;
                case 31:
                    Debug.Log("Achievement 1/5");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_5");
                    break;
                case 36:
                    Debug.Log("Achievement 1/6");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_6");
                    break;
                case 41:
                    Debug.Log("Achievement 1/7");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_7");
                    break;
                case 46:
                    Debug.Log("Achievement 1/8");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_8");
                    break;
                case 51:
                    Debug.Log("Achievement 1/9");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_9");
                    break;
                case 56:
                    Debug.Log("Achievement 1/10");
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

        foreach (ButtonGame button in buttonsInvisible)
        {
            animatorBtn = button.GetComponent<Animator>();
            animatorBtn.SetBool("Invisible", true);
            animatorBtn.SetBool("ButtonOff", false);
            yield return new WaitForSeconds(0.3f);
        }

    }

    IEnumerator TeleportButtons()
    {
        Animator animatorBtn;
        if(buttonsTeleport.Count == 0)
        {
            allowPlayerControl = true;
            yield return new WaitForSeconds(0f);
        } else
        {
            yield return new WaitForSeconds(0.3f);
            foreach (ButtonGame button in buttonsTeleport)
            {
                animatorBtn = button.GetComponent<Animator>();
                StartCoroutine(button.Teleport(animatorBtn));
                yield return new WaitForSeconds(1f);
            }

            allowPlayerControl = true;
        }
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
        StartCoroutine(PlayVictoryAnimation());

        int i = 0;

        foreach (GameObject objectToReveal in listObjectsToReveal)
        {
            objectToReveal.SetActive(true);

            if (i < buttonsFiltered.Count) {
                // Turn off buttons lights when revealing stars
                buttonsFiltered[i].gameObject.GetComponentInChildren<Light2D>().intensity = 0;
            }

            i++;
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

        if(levelSaved < level && level < 62)
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

    IEnumerator PlayVictoryAnimation()
    {
        Instantiate(animationWonPrefab, new Vector3(2.56f, 0.67f, 0f), Quaternion.identity);
        soundController.PlayAudioOnce(starSounds[Random.Range(0, 2)], 0.1f);

        for (int i = 0; i < 7; i++)
        {
            yield return new WaitForSeconds(0.9f);
            Instantiate(animationWonPrefab, new Vector3(Random.Range(-4f, 9.5f), Random.Range(-3f, 3.2f), 0f), Quaternion.identity);
            soundController.PlayAudioOnce(starSounds[Random.Range(0, 2)], 0.1f);
        }
    }

    void getButtonsInvisible()
    {
        buttonsInvisible = new List<ButtonGame>();
        foreach (ButtonGame button in listButtonsComplete)
        {
            if (button.isInvisible)
            {
                buttonsInvisible.Add(button);
            }

        }
    }

    void getButtonsTeleport()
    {
        buttonsTeleport = new List<ButtonGame>();
        foreach (ButtonGame button in listButtonsComplete)
        {
            if (button.canTeleport)
            {
                buttonsTeleport.Add(button);
            }

        }
    }

}