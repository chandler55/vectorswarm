using UnityEngine;
using System.Collections;
using GooglePlayGames;

public class ToggleLeaderboardsMenu : MonoBehaviour
{
    void OnClick()
    {
        if ( Social.localUser.authenticated )
        {
#if UNITY_ANDROID
            ( (PlayGamesPlatform)Social.Active ).ShowLeaderboardUI( "CgkI6ZDq1r0FEAIQAA" );
#elif UNITY_IPHONE
            Social.Active.ShowLeaderboardUI();
#endif
        }
        else
        {
            Messenger.Broadcast( Events.OnlineEvents.TryAuthenticateUser, true );
        }
    }
}
