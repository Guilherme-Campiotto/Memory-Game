using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

            soundController.PlayAudioOnce(soundController.soundtrack[soundController.numberOfCurrentSong], 0.8f);
            soundController.nextSongQueue = false;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        gameController.ResetButtons();
    }

    public void NextLevel()
    {
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
        GameConfiguration.fixedBackground = true;
        GenerateRandomBackgroundColorCamera();
        GenerateRandomBackgroundColorCanvasImage();
    }

    public void OpenLevelSelectMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenLastPlayedLevel()
    {
        int playerCurrentScene = PlayerPrefs.GetInt("PlayerProgress");

        if (playerCurrentScene == 0)
        {
            playerCurrentScene = 2;
            PlayerPrefs.SetInt("PlayerProgress", playerCurrentScene);
        }

        SceneManager.LoadScene(playerCurrentScene);
    }

    public void OpenCredits()
    {

    }

}
