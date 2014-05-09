using UnityEngine;
using System.Collections;

public class Scoring : MonoBehaviour
{
    private long mHighScore = 0;
    private int mMultiplier = 1;

    private Vector3 mScoreOffset = new Vector3( 0, -2.5f, 0 );
    void Start()
    {
        mMultiplier = 1;

        Messenger.AddListener<Vector3>( Events.GameEvents.IncrementMultipler, OnIncrementMultiplier );
        Messenger.AddListener<long, Vector3>( Events.GameEvents.IncrementScore, OnIncrementScore );

    }

    void OnDestroy()
    {
        Messenger.RemoveListener<Vector3>( Events.GameEvents.IncrementMultipler, OnIncrementMultiplier );
        Messenger.RemoveListener<long, Vector3>( Events.GameEvents.IncrementScore, OnIncrementScore );
    }

    void Update()
    {

    }

    void OnIncrementScore( long score, Vector3 worldPos )
    {
        long addScore = mMultiplier * score;
        mHighScore += addScore;

        worldPos += mScoreOffset;
        GameObject go = EntityDatabase.Instance.CreateEntity( EntityDatabase.EntityType.EntityType_ScoreIndicator, worldPos, Quaternion.identity );
        ScoreIndicator scoreIndicatorComponent = go.GetComponent<ScoreIndicator>();
        if ( scoreIndicatorComponent )
        {
            scoreIndicatorComponent.SetScore( addScore );
        }

        Messenger.Broadcast<long>( Events.UIEvents.HighScoreUpdated, mHighScore );
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
}
