using UnityEngine;
using System.Collections;
using GooglePlayGames;

public class ToggleLeaderboardsMenu : MonoBehaviour
{
    void OnClick()
    {
        if ( Social.localUser.authenticated )
        {
            ( (PlayGamesPlatform) Social.Active ).ShowLeaderboardUI( "CgkI6ZDq1r0FEAIQAA" );
        }
        else
        {
            Messenger.Broadcast( Events.OnlineEvents.TryAuthenticateUser, true );
        }
    }
}
