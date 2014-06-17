using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public int zLayer = -10;
    public GameObject tutorialScreenPrefab;

    private GameObject  mTutorialScreen = null;
    private bool        mCurrentlyShown = false;

    void Start()
    {
        GameUtils.Assert( tutorialScreenPrefab );
        Messenger.AddListener( Events.UIEvents.ToggleTutorialScreen, OnToggleTutorialScreen );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.UIEvents.ToggleTutorialScreen, OnToggleTutorialScreen );
    }

    public void ToggleTutorialScreen()
    {
        if ( mCurrentlyShown )
        {
            mCurrentlyShown = false;

            if ( mTutorialScreen != null )
            {
                mTutorialScreen.SetActive( false );
            }
        }
        else
        {
            mCurrentlyShown = true;

            if ( mTutorialScreen == null )
            {
                mTutorialScreen = Instantiate( tutorialScreenPrefab ) as GameObject;
                mTutorialScreen.transform.parent = transform;
                mTutorialScreen.transform.localPosition = Vector3.zero + new Vector3( 0, 0, zLayer );
            }
            else
            {
                mTutorialScreen.SetActive( true );
            }
        }
    }

    void OnToggleTutorialScreen()
    {
        ToggleTutorialScreen();
    }
}
