﻿using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GPGManager : MonoBehaviour
{
    void Start()
    {
        // recommended for debugging:
        if ( Debug.isDebugBuild )
        {
            PlayGamesPlatform.DebugLogEnabled = true;
        }

        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        Messenger.AddListener<bool>( Events.OnlineEvents.TryAuthenticateUser, TryAuthenticate );

        /*
        if ( SaveData.current != null && !SaveData.current.socialAuthenticated )
        {
            SaveData.current.socialAuthenticated = true;
            SaveLoad.Save();
        }
        */

        TryAuthenticate( false );
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
                    ( (PlayGamesPlatform)Social.Active ).ShowLeaderboardUI( "CgkI6ZDq1r0FEAIQAA" );
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
