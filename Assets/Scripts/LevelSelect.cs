using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public List<Button> listButtons;
    public List<Image> listImagesLocked;
    private int playerCurrentScene;
    public SoundController soundController;
    public AudioClip menuClick3;

    void Start()
    {
        GetPlayerProgress();
        SetImagesLockedStage();
        SetStageButtons();

        GameObject soundObject = GameObject.Find("SoundController");

        if (soundObject != null)
        {
            soundController = soundObject.GetComponent<SoundController>();
        }

        //PlayerPrefs.DeleteKey("PlayerProgress"); // apaga o progresso
    }

    void GetPlayerProgress()
    {
        playerCurrentScene = PlayerPrefs.GetInt("PlayerProgress");

        if (playerCurrentScene == 0)
        {
            playerCurrentScene = 2;
            PlayerPrefs.SetInt("PlayerProgress", playerCurrentScene);
        }
    }

    void GoToStage(int stageIndex)
    {
        if (playerCurrentScene >= stageIndex)
        {
            soundController.PlayAudioOnce(menuClick3, 0.8f);
            SceneManager.LoadScene(stageIndex);
        }
    }

    void SetImagesLockedStage()
    {
        int sceneIndex = 3;
        foreach (Image lockedImage in listImagesLocked)
        {
            if (sceneIndex <= playerCurrentScene)
            {
                Destroy(lockedImage.gameObject);
            }

            sceneIndex++;
        }
    }

    void SetStageButtons()
    {
        int btnIndex = 2;
        foreach (Button button in listButtons)
        {
            int currentIndex = btnIndex;
            button.onClick.AddListener(delegate { GoToStage(currentIndex); });
            btnIndex++;
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
