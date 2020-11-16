using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMode : MonoBehaviour
{
    public float fps;
    public float timeTotal;
    public float timeLevel;
    public float timeLevelSaved;
    public float numberOfTries;
    public float deltaTime;

    string fpsString;
    string timeTotalString;
    string timeLevelString;
    string numberOfTriesString;

    public Text txtFps;
    public Text txtTimeTotal;
    public Text txtTimeLevel;
    public Text txtNumberOfTries;

    public bool stopCountLevel = false;

    // Start is called before the first frame update
    void Start()
    {
        fpsString = txtFps.text;
        timeTotalString = txtTimeTotal.text;
        timeLevelString = txtTimeLevel.text;
        numberOfTriesString = txtNumberOfTries.text;

        timeLevel = timeLevelSaved = PlayerPrefs.GetFloat("LevelCurrentTime");
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;

        if(!stopCountLevel)
        {
            timeLevel = Time.time - timeLevelSaved;
        }

        ConvertToText();
    }

    void ConvertToText()
    {
        txtFps.text = fpsString + Mathf.Ceil(fps).ToString();
        txtTimeTotal.text = timeTotalString + Time.time.ToString("f2");
        txtTimeLevel.text = timeLevelString + timeLevel.ToString("f2");

    }
}
