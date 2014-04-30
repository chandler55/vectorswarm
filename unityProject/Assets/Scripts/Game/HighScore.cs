using UnityEngine;
using System.Collections;

public class HighScore : MonoBehaviour
{

    private long mScore = 0;

    void Start()
    {
        Messenger.AddListener<long>( Events.GameEvents.IncrementScore, OnScoreUpdate );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<long>( Events.GameEvents.IncrementScore, OnScoreUpdate );
    }

    void Update()
    {

    }

    void OnScoreUpdate( long score )
    {
        mScore += score;

        Messenger.Broadcast<long>( Events.UIEvents.HighScoreUpdated, mScore );
    }
}
