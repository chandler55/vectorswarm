using UnityEngine;
using System.Collections;

using GoogleMobileAds.Api;
using Soomla.Store;

public class AdManager : MonoBehaviour
{
#if UNITY_ANDROID
    public static string adIdentifier = "ca-app-pub-6248233767469489/4911567496";
#elif UNITY_IPHONE
	public static string adIdentifier = "ca-app-pub-6248233767469489/6621973091";
#else
	public static string adIdentifier = "";
#endif

    BannerView bannerView = null;
    private bool adsOn = true;

    void Start()
    {
        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView( adIdentifier, AdSize.Banner, AdPosition.Bottom );

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().AddTestDevice( "18c368de3bc4d699be3b12c3b00e2eec" ).Build();

        // Load the banner with the request.
        if ( bannerView != null )
        {
            bannerView.LoadAd( request );
        }

        Messenger.AddListener( Events.GameEvents.PlayerDied, OnPlayerDeath );
        Messenger.AddListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
        Messenger.AddListener( Events.StoreEvents.StoreInitialized, OnStoreInitialized );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.GameEvents.PlayerDied, OnPlayerDeath );
        Messenger.RemoveListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
        Messenger.RemoveListener( Events.StoreEvents.StoreInitialized, OnStoreInitialized );

        bannerView.Destroy();
    }

    void Update()
    {

    }

    void OnPlayerDeath()
    {
        if ( bannerView != null && adsOn )
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

    void OnStoreInitialized()
    {
        bool hasNoAds = StoreInventory.NonConsumableItemExists( VectorSwarmAssets.NO_ADDS_NONCONS_PRODUCT_ID );
        if ( hasNoAds )
        {
            adsOn = false;
            bannerView.Hide();
        }
    }
}
