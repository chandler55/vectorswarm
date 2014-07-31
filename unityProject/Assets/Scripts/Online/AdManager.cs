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

    public static string interstitialAdIdentifier = "ca-app-pub-6248233767469489/4289761093";

    BannerView bannerView = null;
    InterstitialAd interstitialAd = null;

    void Start()
    {
        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView( adIdentifier, AdSize.Banner, AdPosition.Bottom );

        // create interstitial
        interstitialAd = new InterstitialAd( interstitialAdIdentifier );

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().AddTestDevice( "18c368de3bc4d699be3b12c3b00e2eec" ).Build();

        /*
        // Load the banner with the request.
        if ( bannerView != null )
        {
            bannerView.LoadAd( request );
        }
         */

        if ( interstitialAd != null )
        {
            interstitialAd.LoadAd( request );
        }

        Messenger.AddListener( Events.MenuEvents.OnShowMainMenuFinished, OnMainMenuShown );
        Messenger.AddListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
        Messenger.AddListener( Events.StoreEvents.StoreInitialized, OnStoreInitialized );
        Messenger.AddListener( Events.StoreEvents.NoAdsPurchased, OnNoAdsPurchased );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.MenuEvents.OnShowMainMenuFinished, OnMainMenuShown );
        Messenger.RemoveListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
        Messenger.RemoveListener( Events.StoreEvents.StoreInitialized, OnStoreInitialized );
        Messenger.RemoveListener( Events.StoreEvents.NoAdsPurchased, OnNoAdsPurchased );

        bannerView.Destroy();
        interstitialAd.Destroy();
    }

    void Update()
    {

    }

    void OnMainMenuShown()
    {
        if ( !SaveData.current.noAdsUnlocked )
        {
            if ( interstitialAd != null && interstitialAd.IsLoaded() )
            {
                interstitialAd.Show();
            }
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
            if ( bannerView != null )
            {
                bannerView.Hide();
            }
        }
    }

    void OnNoAdsPurchased()
    {
        if ( bannerView != null )
        {
            bannerView.Hide();
        }
    }
}
