using UnityEngine;
using System.Collections;

public class GameHudUI : MonoBehaviour
{
    public GameObject gameHUD;
    public GameObject menuHUD;

    void Start()
    {
        GameUtils.Assert( gameHUD );
        GameUtils.Assert( menuHUD );

        menuHUD.SetActive( false );
        gameHUD.SetActive( true );

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
    }

    void OnNewGameStarted()
    {
        menuHUD.SetActive( false );
        gameHUD.SetActive( true );
    }
}
