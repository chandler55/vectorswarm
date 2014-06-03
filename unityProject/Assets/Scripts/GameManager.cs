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

    private int mRemainingLives = 0;

    void Start()
    {
        Application.targetFrameRate = 60;

        // listen for events
        Messenger.AddListener( Events.GameEvents.RetryLevel, OnRetryLevel );
        Messenger.AddListener( Events.GameEvents.PlayerDied, OnPlayerDied );

        mRemainingLives = 5;
        Messenger.Broadcast<int>( Events.UIEvents.RemainingLivesUpdated, mRemainingLives );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.GameEvents.RetryLevel, OnRetryLevel );
        Messenger.RemoveListener( Events.GameEvents.PlayerDied, OnPlayerDied );
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
        //Messenger.Broadcast( Events.GameEvents.GameStart );
        //Messenger.Broadcast( Events.GameEvents.NewGameStarted );

        m_gameState = GameState.GameState_Start;
    }

    void OnRetryLevel()
    {
        Messenger.Broadcast( Events.GameEvents.NewGameStarted );
        m_gameState = GameState.GameState_Start;
    }

    void OnPlayerDied()
    {
        m_gameState = GameState.GameState_GameOver;

        // check high score
        Score score = GetComponent<Score>();
        if ( score )
        {
            int playerScore = score.GetScore();
            if ( playerScore > SaveData.current.highScore )
            {
                SaveData.current.highScore = playerScore;
                SaveLoad.Save();

                Messenger.Broadcast<long>( Events.UIEvents.HighScoreUpdated, SaveData.current.highScore );
            }
        }
    }
}
