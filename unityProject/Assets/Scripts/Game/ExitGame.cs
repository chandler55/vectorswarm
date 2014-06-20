using UnityEngine;
using System.Collections;

public class ExitGame : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Escape ) )
        { 
            Application.Quit(); 
        }
    }
}
