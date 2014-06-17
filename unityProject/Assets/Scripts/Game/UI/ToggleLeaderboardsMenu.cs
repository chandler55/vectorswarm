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
            ( (PlayGamesPlatform)Social.Active ).ShowLeaderboardUI( GPGManager.leaderboardID );
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
