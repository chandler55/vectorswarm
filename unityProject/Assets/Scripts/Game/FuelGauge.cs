using UnityEngine;
using System.Collections;

public class FuelGauge : MonoBehaviour
{
    private tk2dClippedSprite mGaugeSprite = null;
    private Transform mGaugeTransform = null;

    void Start()
    {
        mGaugeSprite = GetComponentInChildren<tk2dClippedSprite>();

        if ( mGaugeSprite )
        {
            mGaugeTransform = mGaugeSprite.transform;
        }

        Messenger.AddListener<float>( Events.UIEvents.FuelGaugeUpdated, OnSetFuelGauge );

    }

    void OnDestroy()
    {
        Messenger.RemoveListener<float>( Events.UIEvents.FuelGaugeUpdated, OnSetFuelGauge );
    }

    void OnSetFuelGauge( float pct )
    {
        mGaugeSprite.ClipRect = new Rect( 0, 0, 1, pct );
    }
}
