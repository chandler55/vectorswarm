using UnityEngine;
using System.Collections;

public class HighScore : MonoBehaviour
{

    private int mScore = 0;

    void Start()
    {
        Messenger.AddListener<int>( Events.GameEvents.IncrementScore, OnScoreUpdate );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<int>( Events.GameEvents.IncrementScore, OnScoreUpdate );
    }

    void Update()
    {

    }

    void OnScoreUpdate( int score )
    {
        mScore += score;

        Messenger.Broadcast<int>( Events.UIEvents.HighScoreUpdated, mScore );
    }
}
