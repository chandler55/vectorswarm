using UnityEngine;
using System.Collections;

public class ObjectiveBox : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    public GameObject container;

    void Start()
    {
        GameUtils.Assert( startPoint );
        GameUtils.Assert( endPoint );
        GameUtils.Assert( container );
        DisableObjective( null );

        Messenger.AddListener( Events.GameEvents.ShowObjective, OnShowObjective );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.GameEvents.ShowObjective, OnShowObjective );
    }

    void OnShowObjective()
    {
        ShowObjective();
    }

    void ShowObjective()
    {
        container.transform.position = startPoint.transform.position;
        Go.to( container.transform, 0.7f, new GoTweenConfig().position( endPoint.position ).setEaseType( GoEaseType.BackOut ).setDelay( 0.5f ) ).setOnCompleteHandler( HideObjective );
        EnableObjective();
    }

    void HideObjective( AbstractGoTween tween )
    {
        Go.to( container.transform, 0.7f, new GoTweenConfig().position( startPoint.position ).setEaseType( GoEaseType.BackIn ).setDelay( 2.0f ) ).setOnCompleteHandler( DisableObjective );
    }

    void HideObjectiveInstant()
    {
        DisableObjective( null );
    }

    void EnableObjective()
    {
        container.SetActive( true );
    }

    void DisableObjective( AbstractGoTween tween )
    {
        Messenger.Broadcast( Events.GameEvents.ObjectiveShown );
        container.SetActive( false );
    }
}
