using UnityEngine;
using System.Collections;

public class GameOverBox : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    public GameObject container;

    void Start()
    {
        GameUtils.Assert( startPoint );
        GameUtils.Assert( endPoint );
        GameUtils.Assert( container );

        DisableGameOver( null );

        Messenger.AddListener( Events.GameEvents.ShowGameOver, OnShowGameOver );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.GameEvents.ShowGameOver, OnShowGameOver );
    }

    void OnShowGameOver()
    {
        ShowGameOver();
    }

    void ShowGameOver()
    {
        container.transform.position = startPoint.transform.position;
        Go.to( container.transform, 0.7f, new GoTweenConfig().position( endPoint.position ).setEaseType( GoEaseType.BackOut ).setDelay( 0.5f ) );
        EnableGameOver();
    }

    void HideGameOver( AbstractGoTween tween )
    {
        Go.to( container.transform, 0.7f, new GoTweenConfig().position( startPoint.position ).setEaseType( GoEaseType.BackIn ).setDelay( 2.0f ) ).setOnCompleteHandler( DisableGameOver );
    }

    void HideGameOverInstant()
    {
        DisableGameOver( null );
    }

    void EnableGameOver()
    {
        container.SetActive( true );
    }

    void DisableGameOver( AbstractGoTween tween )
    {
        container.SetActive( false );
    }
}
