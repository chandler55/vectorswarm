using UnityEngine;
using System.Collections;

public class FuelGauge : MonoBehaviour
{
    public tk2dSlicedSprite mGaugeSprite = null;
    public tk2dSprite   mLightningBolt = null;
    public Color        mGaugeAlternatingColor = Color.white;

    private Transform   mGaugeTransform = null;
    private float       mGaugeFilledSizeX = 0.0f;

    // animating lightning bolt flash when full
    private bool        mIsFull = false;
    private float       mBoltAlpha = 1.0f;
    private bool        mBoltDarkening = true;
    private float       mBoltFlashingDuration = 0.5f; // in seconds

    private Color       mOriginalGaugeColor = Color.white;

    void Start()
    {
        GameUtils.Assert( mGaugeSprite );
        GameUtils.Assert( mLightningBolt );

        if ( mGaugeSprite )
        {
            mGaugeTransform = mGaugeSprite.transform;
            mGaugeFilledSizeX = mGaugeSprite.dimensions.x;
            mOriginalGaugeColor = mGaugeSprite.color;
        }

        Messenger.AddListener<float>( Events.UIEvents.FuelGaugeUpdated, OnSetFuelGauge );

        mIsFull = true;
    }

    void Update()
    {
        if ( mIsFull )
        {
            if ( mBoltDarkening )
            {
                mBoltAlpha -= Time.deltaTime * ( 1.0f / mBoltFlashingDuration );
                if ( mBoltAlpha <= 0.0f )
                {
                    mBoltAlpha = 0.0f;
                    mBoltDarkening = false;
                }
            }
            else
            {
                mBoltAlpha += Time.deltaTime * ( 1.0f / mBoltFlashingDuration );
                if ( mBoltAlpha >= 1.0f )
                {
                    mBoltAlpha = 1.0f;
                    mBoltDarkening = true;
                }
            }

            mLightningBolt.color = new Color( 1.0f, 1.0f, 1.0f, mBoltAlpha );

            // brighten the gauge as well
            //mGaugeSprite.color = mBoltAlpha * mGaugeAlternatingColor + mOriginalGaugeColor * ( 1 - mBoltAlpha );
        }
        else
        {
            mLightningBolt.color = Color.white;
            mGaugeSprite.color = mOriginalGaugeColor;
        }
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<float>( Events.UIEvents.FuelGaugeUpdated, OnSetFuelGauge );
    }

    void OnSetFuelGauge( float pct )
    {
        mIsFull = pct == 1.0f;
        mGaugeSprite.dimensions = new Vector2( pct * mGaugeFilledSizeX, 38 );
    }
}
