using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                Debug.Log("dentro do ray");
                // whatever tag you are looking for on your game object
                if (hit.collider.tag == "ButtonGame")
                {
                    Debug.Log("---> Hit: ");
                }
            }
        }
        */
        
    }
}
