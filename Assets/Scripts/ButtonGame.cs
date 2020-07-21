using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        Debug.Log("Mouse Over");
        if (Input.GetMouseButtonDown(0)) {
            // Whatever you want it to do.
            Debug.Log("---> Hit: ");
        }
    }
}
