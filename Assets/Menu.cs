using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text level;
    int levelNumber;

    // Start is called before the first frame update
    void Start()
    {
        levelNumber = SceneManager.GetActiveScene().buildIndex + 1;
        
        if (level != null)
        {
            level.text = level.text + levelNumber;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
