using UnityEngine;
using System.Collections;

public class MovementLogic : MonoBehaviour
{
    public float    mEasingAmount = 5.0f;
    public float    mThresholdForAccelerometer = 0.04f;
    private float   mPreviousAccelerationX = 0.0f;

    private PlayerSnake mPlayerSnake;

    void Start()
    {
        mPlayerSnake = gameObject.GetComponent<PlayerSnake>();
        GameUtils.Assert( mPlayerSnake );
    }

    void Update()
    {

        //Player Horizontal Movement
        {
#if UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8

            // move towards accelerometer tilt
            // tilt to horizontal percentage -0.25 to 0.25
            float accelerationValueToUse = 0.0f;

            if ( Mathf.Abs( mPreviousAccelerationX - Input.acceleration.x ) < mThresholdForAccelerometer )
            {
                accelerationValueToUse = mPreviousAccelerationX;
            }
            else
            {
                accelerationValueToUse = Input.acceleration.x;
            }

            float percentage = Mathf.InverseLerp( -0.25f, 0.25f, accelerationValueToUse );
            mPreviousAccelerationX = accelerationValueToUse;

            Vector2 tiltPos = new Vector2( Boundaries.Instance.GetPercentageToPosition( percentage ), 0 );
            Vector2 target = tiltPos - mPlayerSnake.Position;

            /**/
#else
            // move towards mouse
            Vector2 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );

            // lock target within screen 
            Boundaries.Instance.ClampHorizontal( ref mousePos );
            Vector2 target = mousePos - mPlayerSnake.Position;
#endif

            /**/
            mPlayerSnake.Velocity = target * mEasingAmount;
        }
    }
}
