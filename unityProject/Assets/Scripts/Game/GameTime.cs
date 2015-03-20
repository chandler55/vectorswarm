using UnityEngine;
using System.Collections;

public class GameTime : MonoBehaviour
{
    private const float DURATION = 120.0f;
    private float mTimerInSeconds = 0.0f;
    private int mSecondsLeft = 0;

    void Start()
    {
        Messenger.AddListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
    }

    void Update()
    {
        if ( mTimerInSeconds > 0.0f )
        {
            mTimerInSeconds -= Time.deltaTime;
            int originalSeconds = mSecondsLeft;
            mSecondsLeft = Mathf.CeilToInt( mTimerInSeconds );

            // refresh ui on change
            if ( originalSeconds != mSecondsLeft )
            {
                Messenger.Broadcast<int>( Events.UIEvents.RefreshTimeLeft, mSecondsLeft );
            }
        }
        else
        {
            mTimerInSeconds = 0.0f;
        }
    }

    void OnNewGameStarted()
    {
        mTimerInSeconds = DURATION;
        mSecondsLeft = Mathf.CeilToInt( mTimerInSeconds );
    }
}
