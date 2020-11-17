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
    public ColorPaletteComponent colorPaletteCanvas;
    public ColorPaletteComponent colorPaletteCamera;
    int levelNumber;

    public GameObject btnRestart;
    public GameObject btnNext;

    void Start()
    {
        levelNumber = SceneManager.GetActiveScene().buildIndex + 1;

        GameObject soundObject = GameObject.Find("SoundController");
        GameObject gameControllerObject = GameObject.Find("GameController");
        GameObject colorPaletteCanvasObject = GameObject.Find("[UI_Image]");
        GameObject colorPaletteCameraObject = GameObject.Find("Main Camera");

        if (soundObject != null)
        {
            soundController = soundObject.GetComponent<SoundController>();
        }

        if(gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if(!GameConfiguration.fixedBackground)
        {
            if(colorPaletteCanvasObject != null)
            {
                colorPaletteCanvas = colorPaletteCanvasObject.GetComponent<ColorPaletteComponent>();

                GenerateRandomBangroundColor(colorPaletteCanvas);
            }

            if (colorPaletteCameraObject != null)
            {
                colorPaletteCamera = colorPaletteCameraObject.GetComponent<ColorPaletteComponent>();

                GenerateRandomBangroundColor(colorPaletteCamera);
            }
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
    void GenerateRandomBangroundColor(ColorPaletteComponent colorPalette)
    {
        if(colorPalette != null)
        {
            colorPalette.ColorPaletteIndex = Random.Range(0, ColorPaletteComponent.CurrentPaletteData.ColorList.Count);
            colorPalette.OnRefreshColorPalette();
        }

    }

    public void ChangeBackground()
    {
        GameConfiguration.fixedBackground = true;
        GenerateRandomBangroundColor(colorPaletteCanvas);
        GenerateRandomBangroundColor(colorPaletteCamera);
    }

    public void OpenLevelSelectMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenLevelSelected()
    {
        // mudar depois para ficar dinamico e funcionar em qualquer fase
        SceneManager.LoadScene(2);
    }

    public void OpenCredits()
    {

    }
}
