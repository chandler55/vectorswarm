using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        GameState_Start,
        GameState_ShowObjective,
        GameState_Playing,
        GameState_GameOver,
    }

    private GameState m_gameState = GameState.GameState_Start;
    public GameState CurrentGameState
    {
        get
        {
            return m_gameState;
        }
    }

    private bool mInitialized = false;

    private int waitFrames = 0;

    void Start()
    {
        Application.targetFrameRate = 60;

        // listen for events
        Messenger.AddListener( Events.GameEvents.ObjectiveShown, OnObjectiveShown );
        Messenger.AddListener( Events.GameEvents.RetryLevel, OnRetryLevel );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.GameEvents.ObjectiveShown, OnObjectiveShown );
        Messenger.RemoveListener( Events.GameEvents.RetryLevel, OnRetryLevel );
    }

    void Update()
    {
        waitFrames++;

        if ( !mInitialized && waitFrames == 1 )
        {
            mInitialized = true;
            LoadLevel();
        }
    }

    void LoadLevel()
    {
        Messenger.Broadcast( Events.GameEvents.GameStart );
        Messenger.Broadcast( Events.GameEvents.ShowObjective );

        m_gameState = GameState.GameState_ShowObjective;
    }

    void OnRetryLevel()
    {
        Application.LoadLevel( "start" );
    }

    void OnObjectiveShown()
    {
        m_gameState = GameState.GameState_Playing;
    }

    void CheckObjectives()
    {

    }
}
