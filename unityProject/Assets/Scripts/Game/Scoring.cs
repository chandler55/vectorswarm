using UnityEngine;
using System.Collections;

public class Scoring : MonoBehaviour
{
    private long mScore = 0;
    private int mMultiplier = 1;

    private Vector3 mScoreOffset = new Vector3( 0, -2.5f, 0 );
    void Start()
    {
        Messenger.AddListener( Events.GameEvents.NewGameStarted, OnNewGameStart );
        Messenger.AddListener<Vector3>( Events.GameEvents.IncrementMultipler, OnIncrementMultiplier );
        Messenger.AddListener<long, Vector3>( Events.GameEvents.IncrementScore, OnIncrementScore );
        Messenger.AddListener( Events.GameEvents.PlayerDied, OnPlayerDeath );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.GameEvents.NewGameStarted, OnNewGameStart );
        Messenger.RemoveListener<Vector3>( Events.GameEvents.IncrementMultipler, OnIncrementMultiplier );
        Messenger.RemoveListener<long, Vector3>( Events.GameEvents.IncrementScore, OnIncrementScore );
        Messenger.RemoveListener( Events.GameEvents.PlayerDied, OnPlayerDeath );
    }

    void OnPlayerDeath()
    {
        // post to gpg leaderboards
        if ( Social.localUser.authenticated )
        {
            Social.ReportScore( mScore, GPGManager.leaderboardID, OnScorePosted );
        }

        Messenger.Broadcast<long>( Events.GameEvents.PostGameOverScore, mScore );
    }

    void OnScorePosted( bool success )
    {
        if ( success )
        {
            Debug.Log( "score posting failed" );
        }
        else
        {
            Debug.Log( "failed to post score" );
        }
    }

    void OnIncrementScore( long score, Vector3 worldPos )
    {
        long addScore = mMultiplier * score;
        mScore += addScore;

        worldPos += mScoreOffset;
        GameObject go = EntityDatabase.Instance.CreateEntity( EntityDatabase.EntityType.EntityType_ScoreIndicator, worldPos, Quaternion.identity );
        ScoreIndicator scoreIndicatorComponent = go.GetComponent<ScoreIndicator>();
        if ( scoreIndicatorComponent )
        {
            scoreIndicatorComponent.SetScore( addScore );
        }

        Messenger.Broadcast<long>( Events.UIEvents.ScoreUpdated, mScore );
    }

    void OnIncrementMultiplier( Vector3 worldPos )
    {
        mMultiplier++;

        worldPos += mScoreOffset;
        GameObject go = EntityDatabase.Instance.CreateEntity( EntityDatabase.EntityType.EntityType_ScoreIndicator, worldPos, Quaternion.identity );
        ScoreIndicator scoreIndicatorComponent = go.GetComponent<ScoreIndicator>();
        if ( scoreIndicatorComponent )
        {
            scoreIndicatorComponent.SetMultiplier( mMultiplier );
        }

        Messenger.Broadcast<int>( Events.UIEvents.MultiplierUpdated, mMultiplier );
    }

    void OnNewGameStart()
    {
        mScore = 0;
        mMultiplier = 1;

        Messenger.Broadcast<long>( Events.UIEvents.ScoreUpdated, mScore );
        Messenger.Broadcast<int>( Events.UIEvents.MultiplierUpdated, mMultiplier );
    }
}
