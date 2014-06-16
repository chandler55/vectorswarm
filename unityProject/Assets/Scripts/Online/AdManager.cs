﻿using UnityEngine;
using System.Collections;

using GoogleMobileAds.Api;
public class AdManager : MonoBehaviour
{
    BannerView bannerView = null;

    void Start()
    {
        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView( "ca-app-pub-6248233767469489/4977557899", AdSize.Banner, AdPosition.Bottom );

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        if ( bannerView != null )
        {
            bannerView.LoadAd( request );
        }

        Messenger.AddListener( Events.GameEvents.PlayerDied, OnPlayerDeath );
        Messenger.AddListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.GameEvents.PlayerDied, OnPlayerDeath );
        Messenger.RemoveListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );

        bannerView.Destroy();
    }

    void Update()
    {

    }

    void OnPlayerDeath()
    {
        if ( bannerView != null )
        {
            bannerView.Show();
        }
    }

    void OnNewGameStarted()
    {
        if ( bannerView != null )
        {
            bannerView.Hide();
        }
    }
}