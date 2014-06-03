using UnityEngine;
using System.Collections;

public class GameHudUI : MonoBehaviour
{
    public GameObject gameHUD;
    public GameObject menuHUD;

    public Transform menuDisabledTransform;

    void Start()
    {
        GameUtils.Assert( gameHUD );
        GameUtils.Assert( menuHUD );
        GameUtils.Assert( menuDisabledTransform );

        menuHUD.SetActive( true );
        gameHUD.SetActive( false );

        Messenger.AddListener( Events.GameEvents.PlayerDied, OnPlayerDeath );
        Messenger.AddListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.GameEvents.PlayerDied, OnPlayerDeath );
        Messenger.RemoveListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
    }

    void Update()
    {

    }

    void OnPlayerDeath()
    {
        menuHUD.SetActive( true );
        gameHUD.SetActive( false );

        menuHUD.transform.localPosition = menuDisabledTransform.localPosition;
        Go.to( menuHUD.transform, 1.5f, new GoTweenConfig().localPosition( Vector3.zero ).setEaseType( GoEaseType.SineOut ) );
    }

    void OnNewGameStarted()
    {
        menuHUD.SetActive( false );
        gameHUD.SetActive( true );
    }
}
