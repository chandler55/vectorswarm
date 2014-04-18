using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{

    public Transform startPoint;
    public Transform endPoint;

    public GameObject container;
    public tk2dSprite blackOverlay;

    private bool mMenuShown = false;

    void Start()
    {
        GameUtils.Assert( startPoint );
        GameUtils.Assert( endPoint );
        GameUtils.Assert( container );
        GameUtils.Assert( blackOverlay );

        Messenger.AddListener( Events.MenuEvents.TriggerPauseMenu, OnLaunchPauseMenu );
        blackOverlay.gameObject.SetActive( false );
        container.transform.position = startPoint.transform.position;
        container.transform.localScale = Vector3.zero;
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.MenuEvents.TriggerPauseMenu, OnLaunchPauseMenu );
    }

    void OnLaunchPauseMenu()
    {
        TriggerPauseMenu();
    }

    public void TriggerPauseMenu()
    {
        if ( !mMenuShown )
        {
            ShowMenu();
        }
        else
        {
            HideMenu();
        }
    }

    void ShowMenu()
    {
        blackOverlay.gameObject.SetActive( true );
        mMenuShown = true;
        Go.to( container.transform, 0.5f, new GoTweenConfig().position( endPoint.position ).setEaseType( GoEaseType.BackOut ) );
        Go.to( container.transform, 0.5f, new GoTweenConfig().scale( 1.0f ) );
    }

    void HideMenu()
    {
        blackOverlay.gameObject.SetActive( false );
        mMenuShown = false;

        Go.to( container.transform, 0.5f, new GoTweenConfig().position( startPoint.position ).setEaseType( GoEaseType.BackIn ) );
        Go.to( container.transform, 0.5f, new GoTweenConfig().scale( 0.0f ) );
    }
}
