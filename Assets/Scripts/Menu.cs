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

    // Start is called before the first frame update
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

        if (level != null)
        {
            level.text = level.text + levelNumber;
        }

        btnRestart.SetActive(false);
        btnNext.SetActive(false);

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
        colorPalette.ColorPaletteIndex = Random.Range(0, ColorPaletteComponent.CurrentPaletteData.ColorList.Count);
        colorPalette.OnRefreshColorPalette();
    }

    public void ChangeBackground()
    {
        GenerateRandomBangroundColor(colorPaletteCanvas);
        GenerateRandomBangroundColor(colorPaletteCamera);
    }
}
