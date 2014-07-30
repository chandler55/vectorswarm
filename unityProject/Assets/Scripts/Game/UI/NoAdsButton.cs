using UnityEngine;
using System.Collections;
using Soomla.Store;

public class NoAdsButton : MonoBehaviour
{
    void Start()
    {
        Messenger.AddListener( Events.StoreEvents.StoreInitialized, OnStoreInitialized );
        Messenger.AddListener( Events.StoreEvents.NoAdsPurchased, OnNoAdsPurchased );
        
        if ( SaveData.current.noAdsUnlocked )
        {
            gameObject.SetActive( false );
        }
        else
        {
            gameObject.SetActive( true );
        }
    }

    void OnDestroy()
    {
        Messenger.AddListener( Events.StoreEvents.StoreInitialized, OnStoreInitialized );
        Messenger.AddListener( Events.StoreEvents.NoAdsPurchased, OnNoAdsPurchased );
    }

    void OnClick()
    {
        Messenger.Broadcast( Events.StoreEvents.PurchaseNoAds );
    }

    void OnStoreInitialized()
    {
        bool hasNoAds = StoreInventory.NonConsumableItemExists( VectorSwarmAssets.NO_ADDS_NONCONS_PRODUCT_ID );
        if ( hasNoAds )
        {
            Debug.Log( "has no ads" );
            gameObject.SetActive( false );
        }
        else
        {
            Debug.Log( "has ads" );
            gameObject.SetActive( true );
        }
    }

    void OnNoAdsPurchased()
    {
        gameObject.SetActive( false );
    }
}
