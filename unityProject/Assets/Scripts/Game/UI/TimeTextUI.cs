using UnityEngine;
using System.Collections;

public class TimeTextUI : MonoBehaviour
{
    public TMPro.TextMeshPro text;

    void Start()
    {
        GameUtils.Assert( text );
        Messenger.AddListener<int>( Events.UIEvents.RefreshTimeLeft, OnTimeLeftRefresh );
    }

    void OnDestroy()
    {
        Messenger.AddListener<int>( Events.UIEvents.RefreshTimeLeft, OnTimeLeftRefresh );
    }

    void OnTimeLeftRefresh( int timeLeft )
    {
        string minutes = Mathf.Floor( timeLeft / 60 ).ToString( "0" );
        string seconds = ( timeLeft % 60 ).ToString( "00" );
        text.text = minutes + ":" + seconds;
    }
}
