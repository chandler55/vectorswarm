using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GPGManager : MonoBehaviour
{
#if UNITY_ANDROID
    public static string leaderboardID = "CgkI6ZDq1r0FEAIQAA";
#elif UNITY_IPHONE
    public static string leaderboardID = "vectorswarmhighscore";
#else
    public static string leaderboardID = "test";
#endif

    void Start()
    {
        // recommended for debugging:
        if ( Debug.isDebugBuild )
        {
            PlayGamesPlatform.DebugLogEnabled = true;
        }

#if UNITY_ANDROID
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
#endif

        Messenger.AddListener<bool>( Events.OnlineEvents.TryAuthenticateUser, TryAuthenticate );

        /*
        if ( SaveData.current != null && !SaveData.current.socialAuthenticated )
        {
            SaveData.current.socialAuthenticated = true;
            SaveLoad.Save();
        }
        */
#if UNITY_ANDROID || UNITY_IPHONE
        TryAuthenticate( false );
#endif
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<bool>( Events.OnlineEvents.TryAuthenticateUser, TryAuthenticate );
    }

    void Update()
    {

    }

    void TryAuthenticate( bool showLeaderboards )
    {
        // authenticate user:
        Social.localUser.Authenticate( ( bool success ) =>
        {
            // handle success or failure
            if ( success )
            {
                if ( showLeaderboards )
                {
#if UNITY_ANDROID
                    ( (PlayGamesPlatform)Social.Active ).ShowLeaderboardUI( GPGManager.leaderboardID );
#elif UNITY_IPHONE
                    Social.Active.ShowLeaderboardUI();
#endif
                }
                Debug.Log( "user authenticated" );
            }
            else
            {
                Debug.Log( "user not authenticated" );
            }
        } );
    }
}
