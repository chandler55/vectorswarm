using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
    private float mScore = 0;

    void Start()
    {
        Messenger.AddListener<float>( Events.GameEvents.IncrementScore, OnScoreUpdate );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<float>( Events.GameEvents.IncrementScore, OnScoreUpdate );
    }


    void OnScoreUpdate( float score )
    {
        int beforeScore = (int)mScore;
        mScore += score;

        // only update if it changed
        if ( beforeScore != (int)mScore )
        {
            Messenger.Broadcast<int>( Events.UIEvents.ScoreUpdated, (int)mScore );
        }
    }

    public int GetScore()
    {
        return (int)mScore;
    }
}
