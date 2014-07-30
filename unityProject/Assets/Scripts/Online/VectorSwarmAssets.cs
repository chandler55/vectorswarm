/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

/// <summary>
/// This class defines our game's economy, which includes virtual goods, virtual currencies
/// and currency packs, virtual categories, and non-consumable items.
/// </summary>
public class VectorSwarmAssets : IStoreAssets
{

    /// <summary>
    /// see parent.
    /// </summary>
    public int GetVersion()
    {
        return 0;
    }

    /// <summary>
    /// see parent.
    /// </summary>
    public VirtualCurrency[] GetCurrencies()
    {
        return new VirtualCurrency[] { };
    }

    /// <summary>
    /// see parent.
    /// </summary>
    public VirtualGood[] GetGoods()
    {
        return new VirtualGood[] { };
    }

    /// <summary>
    /// see parent.
    /// </summary>
    public VirtualCurrencyPack[] GetCurrencyPacks()
    {
        return new VirtualCurrencyPack[] { };
    }

    /// <summary>
    /// see parent.
    /// </summary>
    public VirtualCategory[] GetCategories()
    {
        return new VirtualCategory[] { };
    }


    /// <summary>
    /// see parent.
    /// </summary>
    public NonConsumableItem[] GetNonConsumableItems()
    {
        return new NonConsumableItem[] { NO_ADDS_NONCONS };
    }

    /** Static Final Members **/
    public const string NO_ADDS_NONCONS_PRODUCT_ID   = "no_ads2";

    /** Market MANAGED Items **/

    public static NonConsumableItem NO_ADDS_NONCONS  = new NonConsumableItem(
        "No Ads ",
        "No Ads ",
        NO_ADDS_NONCONS_PRODUCT_ID,
        new PurchaseWithMarket( new MarketItem( NO_ADDS_NONCONS_PRODUCT_ID, MarketItem.Consumable.NONCONSUMABLE, 0.99 ) )
    );

}

