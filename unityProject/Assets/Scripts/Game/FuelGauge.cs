using UnityEngine;
using System.Collections;

public class FuelGauge : MonoBehaviour
{
    private tk2dClippedSprite mGaugeSprite = null;
    private float mOriginalYScale = 0.0f;
    private Transform mGaugeTransform = null;

    void Start()
    {
        mGaugeSprite = GetComponentInChildren<tk2dClippedSprite>();
        if ( mGaugeSprite )
        {
            mGaugeTransform = mGaugeSprite.transform;
            mOriginalYScale = mGaugeTransform.localScale.y;
        }

        Messenger.AddListener<float>( Events.GameEvents.SetFuelGauge, OnSetFuelGauge );

    }

    void OnDestroy()
    {
        Messenger.RemoveListener<float>( Events.GameEvents.SetFuelGauge, OnSetFuelGauge );
    }

    void Update()
    {

    }

    void OnSetFuelGauge( float pct )
    {
        mGaugeSprite.ClipRect = new Rect( 0, 0, 1, pct );
    }
}
