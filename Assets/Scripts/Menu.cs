using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text level;
    public SoundController soundController;
    public GameController gameController;
    public Camera camera;
    public Image canvasImage;
    int levelNumber;

    public GameObject btnRestart;
    public GameObject btnNext;

    public List<Color> colorsList;

    public AudioClip menuClick;
    public AudioClip menuClick2;
    public AudioClip menuClick3;

    public Animator creditsAnimator;

    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public Image imageCanvasSound;
    public bool soundOn = true;
    public GameObject exitPanel;

    void Start()
    {
        levelNumber = SceneManager.GetActiveScene().buildIndex - 1;

        GameObject soundObject = GameObject.Find("SoundController");
        GameObject gameControllerObject = GameObject.Find("GameController");
        GameObject imageCanvasObject = GameObject.Find("[UI_Image]");
        GameObject cameraObject = GameObject.Find("Main Camera");

        if (soundObject != null)
        {
            soundController = soundObject.GetComponent<SoundController>();
        }

        if(gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if (imageCanvasObject != null)
        {
            canvasImage = imageCanvasObject.GetComponent<Image>();
        }

        if (cameraObject != null)
        {
            camera = cameraObject.GetComponent<Camera>();
        }

        if (!GameConfiguration.fixedBackground)
        {
            GenerateRandomBackgroundColorCanvasImage();
            GenerateRandomBackgroundColorCamera();
        }

        if (level != null)
        {
            level.text = level.text + levelNumber;
        }

        if(btnRestart != null) btnRestart.SetActive(false);
        if (btnNext != null) btnNext.SetActive(false);

        if(imageCanvasSound != null) SetSpriteSound();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenCloseExitPanel();
        }
    }

    public void PlayNextSound()
    {
        if(soundController != null)
        {
            soundController.audioSource.Stop();
            soundController.numberOfCurrentSong++;

            if (soundController.numberOfCurrentSong > soundController.numberOfSongs)
            {
                soundController.numberOfCurrentSong = 0;
            }

            soundController.PlayAudioOnce(menuClick2, 0.2f);
            soundController.PlayAudioOnce(soundController.soundtrack[soundController.numberOfCurrentSong], 0.8f);
            soundController.nextSongQueue = false;
        }
    }

    public void QuitGame()
    {
        soundController.PlayAudioOnce(menuClick, 0.7f);
        Application.Quit();
    }

    public void RestartGame()
    {
        soundController.PlayAudioOnce(menuClick2, 0.2f);
        gameController.ResetButtons();
    }

    public void NextLevel()
    {
        soundController.PlayAudioOnce(menuClick2, 0.2f);
        gameController.NextLevel();
    }
    public void ChangeButtonRestartVisibility()
    {
        bool active = btnRestart.activeSelf;
        btnRestart.SetActive(!active);
    }
    public void ChangeButtonNextVisibility()
    {
        bool active = btnNext.activeSelf;
        btnNext.SetActive(!active);
    }
    void GenerateRandomBackgroundColorCamera()
    {
        if(camera != null)
        {
            camera.backgroundColor = GameConfiguration.colorsList[Random.Range(0, GameConfiguration.colorsList.Count)];
        }
    }
    void GenerateRandomBackgroundColorCanvasImage()
    {
        if(canvasImage != null)
        {
            canvasImage.color = GameConfiguration.colorsList[Random.Range(0, GameConfiguration.colorsList.Count)];
        }
    }

    public void ChangeBackground()
    {
        soundController.PlayAudioOnce(menuClick2, 0.2f);
        GameConfiguration.fixedBackground = true;
        GenerateRandomBackgroundColorCamera();
        GenerateRandomBackgroundColorCanvasImage();
    }

    public void OpenLevelSelectMenu()
    {
        soundController.PlayAudioOnce(menuClick2, 0.2f);
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        int playerCurrentScene = PlayerPrefs.GetInt("PlayerProgress");

        if (playerCurrentScene == 0)
        {
            playerCurrentScene = 2;
            PlayerPrefs.SetInt("PlayerProgress", playerCurrentScene);
        }

        soundController.PlayAudioOnce(menuClick, 1);

        StartCoroutine(LoadScene(playerCurrentScene));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneIndex);
    }

    public void OpenCredits()
    {
        soundController.PlayAudioOnce(menuClick, 1);
        creditsAnimator.SetBool("ShowPanel", true);
    }

    public void CloseCredits()
    {
        soundController.PlayAudioOnce(menuClick, 1);
        creditsAnimator.SetBool("ShowPanel", false);
    }
    
    public void MuteUnmuteSound()
    {

        if (GameConfiguration.soundOn)
        {
            GameConfiguration.soundOn = false;
            imageCanvasSound.sprite = soundOffSprite;

        }
        else
        {
            GameConfiguration.soundOn = true;
            imageCanvasSound.sprite = soundOnSprite;
        }

        soundController.ChangeAudioStatus();
    }
    public void SetSpriteSound()
    {
        
        if (GameConfiguration.soundOn)
        {
            imageCanvasSound.sprite = soundOnSprite;
        }
        else
        {
            imageCanvasSound.sprite = soundOffSprite;
        }
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenCloseExitPanel()
    {
        exitPanel.SetActive(!exitPanel.activeSelf);
    }

}
