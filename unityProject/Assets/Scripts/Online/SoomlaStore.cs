using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class SoomlaStore : MonoBehaviour
{
    public List<NonConsumableItem> nonConsumableItems = new List<NonConsumableItem>();

    void Start()
    {
        StoreEvents.OnSoomlaStoreInitialized += onSoomlaStoreInitialized;
        StoreEvents.OnMarketPurchase += onMarketPurchase;

        Soomla.Store.SoomlaStore.Initialize( new VectorSwarmAssets() );

        Messenger.AddListener( Events.StoreEvents.PurchaseNoAds, OnPurchaseNoAds );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.StoreEvents.PurchaseNoAds, OnPurchaseNoAds );
    }

    void onSoomlaStoreInitialized()
    {
        nonConsumableItems = StoreInfo.GetNonConsumableItems();
        Messenger.Broadcast( Events.StoreEvents.StoreInitialized );
        Debug.Log( "store initialized" );
        //Debug.Log( StoreInventory.IsVirtualGoodEquipped( VectorSwarmAssets.NO_ADDS_NONCONS_PRODUCT_ID ) );
    }

    void onMarketPurchase( PurchasableVirtualItem item, string purchaseToken, string payload )
    {
        Debug.Log( "purchased item" );
        if ( item.ItemId == VectorSwarmAssets.NO_ADDS_NONCONS_PRODUCT_ID )
        {
            SaveData.current.noAdsUnlocked = true;
            Messenger.Broadcast( Events.StoreEvents.NoAdsPurchased );
        }
    }

    void OnPurchaseNoAds()
    {
        PurchaseNoAds();
    }

    public void PurchaseNoAds()
    {
        Debug.Log( "try purchase no ads 1" );

        try
        {
            if ( nonConsumableItems.Count > 0 )
            {
                NonConsumableItem item = nonConsumableItems[0];
                if ( item != null )
                {
                    Debug.Log( "try purchase no ads : item id :  " + item.ItemId );
                    StoreInventory.BuyItem( item.ItemId );
                }
            }
        }
        catch ( System.Exception e )
        {
            Debug.Log( "Soomla" + e.Message );
        }
    }

}
