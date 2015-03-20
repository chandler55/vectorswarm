using UnityEngine;
using System.Collections;

public class LoadCheatMenu : MonoBehaviour
{
    public GameObject cheatMenuGameObject;

    void Start()
    {
        GameUtils.Assert( cheatMenuGameObject );
    }

    void Update()
    {
        if ( Debug.isDebugBuild )
        {
            if ( Input.GetKeyDown( KeyCode.M ) || Input.touchCount == 5 )
            {
                cheatMenuGameObject.SetActive( true );
            }
        }
    }
}
