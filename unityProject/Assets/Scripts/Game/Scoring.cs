using UnityEngine;
using System.Collections;

public class Scoring : MonoBehaviour
{
    private long mHighScore = 0;
    private int mMultiplier = 1;

    void Start()
    {
        mMultiplier = 1;

        Messenger.AddListener( Events.GameEvents.IncrementMultipler, OnIncrementMultiplier );
        Messenger.AddListener<long>( Events.GameEvents.IncrementScore, OnIncrementScore );

    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.GameEvents.IncrementMultipler, OnIncrementMultiplier );
        Messenger.RemoveListener<long>( Events.GameEvents.IncrementScore, OnIncrementScore );
    }

    void Update()
    {

    }

    void OnIncrementScore( long score )
    {
        mHighScore += mMultiplier * score;
        Messenger.Broadcast<long>( Events.UIEvents.HighScoreUpdated, mHighScore );
    }

    void OnIncrementMultiplier()
    {
        mMultiplier++;
        Messenger.Broadcast<int>( Events.UIEvents.MultiplierUpdated, mMultiplier );
    }
}
