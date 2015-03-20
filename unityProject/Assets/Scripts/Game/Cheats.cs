using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cheats : MonoBehaviour
{
    Vector2 mScrollView = Vector2.zero;

    void Start()
    {
    }

    void OnGUI()
    {
        Rect guiRect = new Rect( Screen.width * 0.125f, Screen.height * 0.125f, Screen.width * 0.75f, Screen.height * 0.75f );

        // Draw a dark overlay

        GUILayout.BeginArea( guiRect );
        GUILayoutOption buttonOptions = GUILayout.Height( Screen.height * 0.08f );
        GUI.skin.verticalScrollbar.fixedWidth = Screen.width * 0.05f;
        GUI.skin.verticalScrollbarThumb.fixedWidth = Screen.width * 0.05f;

        mScrollView = GUILayout.BeginScrollView( mScrollView );

        if ( GUILayout.Button( "End Timer", buttonOptions ) )
        {
            ExitMenu();
        }

        if ( GUILayout.Button( "Exit", buttonOptions ) )
        {
            ExitMenu();
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    void OnClearAchievements( bool success )
    {

    }

    void ExitMenu()
    {
        gameObject.SetActive( false );
    }
}
