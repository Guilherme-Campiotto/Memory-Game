using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public int buttonExpected = 0;
    public List<ButtonGame> listButtons;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckOrderButtonsPressed(int bottonId)
    {
        Debug.Log("Id do botão pressionado: " + bottonId);
        Debug.Log("Id do botão esperado: " + buttonExpected);
        if(listButtons[buttonExpected].id == bottonId)
        {
            if(listButtons.Count - 1 == buttonExpected)
            {
                Debug.Log("Todos os botões pressionados, proxima fase...");
                NextLevel();
            } else
            {
                Debug.Log("Botão certo");
                buttonExpected += 1;
            }
        } else
        {
            Debug.Log("Botão errado, reiniciando fase...");
            buttonExpected = 0;
        }
    }

    void EndGame()
    {
    
    }

    void NextLevel()
    {

    }
}
