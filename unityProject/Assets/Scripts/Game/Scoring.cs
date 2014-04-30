using UnityEngine;
using System.Collections;

public class Scoring : MonoBehaviour
{
    private int mHighScore = 0;
    private int mMultiplier = 1;

    void Start()
    {
        mMultiplier = 1;

        Messenger.AddListener( Events.GameEvents.IncrementMultipler, OnIncrementMultiplier );
        Messenger.AddListener<int>( Events.GameEvents.IncrementScore, OnIncrementScore );

    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.GameEvents.IncrementMultipler, OnIncrementMultiplier );
        Messenger.RemoveListener<int>( Events.GameEvents.IncrementScore, OnIncrementScore );
    }

    void Update()
    {

    }

    void OnIncrementScore( int score )
    {
        mHighScore += mMultiplier * score;
        Messenger.Broadcast<int>( Events.UIEvents.HighScoreUpdated, mHighScore );
    }

    void OnIncrementMultiplier()
    {
        mMultiplier++;
        Messenger.Broadcast<int>( Events.UIEvents.MultiplierUpdated, mMultiplier );
    }
}
