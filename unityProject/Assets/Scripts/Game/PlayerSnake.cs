using UnityEngine;
using System.Collections;

public class PlayerSnake : Entity
{
    private static PlayerSnake instance;
    public static PlayerSnake Instance
    {
        get
        {
            return instance;
        }
    }

    private float mEasingAmount = 9.0f;
    private float mPlayerNormalSpeed = 15.0f;
    private float mCurrentPlayerSpeedY = 15.0f;

    private Transform mTransform = null;

    // fuel
    private float   mFuelCapacity = 1.0f; // in seconds
    private float   mFuelRemaining = 0.0f;
    private bool    mAfterburnerActivated = false;

    //invincibility
    private float   mFlashInvincibilityDuration = 2.0f;
    private float   mInvincibilityRemaining = 2.0f;
    private bool    mIsInvincible = false;

    void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }

    void Start()
    {
        mTransform = gameObject.transform;
        mFuelRemaining = mFuelCapacity;
    }

    void Update()
    {
        // invincibility logic
        {
            if ( mInvincibilityRemaining > 0 )
            {
                mInvincibilityRemaining -= Time.deltaTime;
                if ( mInvincibilityRemaining <= 0 )
                {
                    mIsInvincible = false;
                    mInvincibilityRemaining = 0.0f;
                }
            }
        }

        // Afterburner Logic
        {
            if ( Input.GetMouseButton( 0 ) && mFuelRemaining == mFuelCapacity )
            {
                mAfterburnerActivated = true;
                mCurrentPlayerSpeedY = 60.0f;
            }

            if ( mAfterburnerActivated )
            {
                mFuelRemaining -= Time.deltaTime;
                if ( mFuelRemaining < 0 )
                {
                    mFuelRemaining = 0;
                    mAfterburnerActivated = false;
                    mCurrentPlayerSpeedY = mPlayerNormalSpeed;

                    // give player temporary invincibility
                    FlashInvincibility();
                }

                Messenger.Broadcast<float>( Events.GameEvents.SetFuelGauge, mFuelRemaining / mFuelCapacity );
            }
            else
            {
                if ( mFuelRemaining <= mFuelCapacity )
                {
                    mFuelRemaining += Time.deltaTime * 0.5f;

                    if ( mFuelRemaining > mFuelCapacity )
                    {
                        mFuelRemaining = mFuelCapacity;
                    }

                    Messenger.Broadcast<float>( Events.GameEvents.SetFuelGauge, mFuelRemaining / mFuelCapacity );
                }
            }
        }

        //Player Horizontal Movement
        {
#if UNITY_ANDROID || UNITY_IPHONE

            // move towards accelerometer tilt
            // tilt to horizontal percentage -0.25 to 0.25
            float percentage = Mathf.InverseLerp( -0.25f, 0.25f, Input.acceleration.x );

            Vector2 tiltPos = new Vector2( Boundaries.Instance.GetPercentageToPosition( percentage ), 0 );
            Vector2 target = tiltPos - Position;
            /**/
#else
            // move towards mouse
            Vector2 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );

            // lock target within screen 
            Boundaries.Instance.ClampHorizontal( ref mousePos );
            Vector2 target = mousePos - Position;
#endif

            /**/
            Velocity = target * mEasingAmount;
        }

        // lock y velocity to mCurrentPlayerSpeedY variable
        Velocity = new Vector2( Velocity.x, mCurrentPlayerSpeedY );
        Position += Velocity * Time.deltaTime;
    }

    public override void CollisionTriggered( Collider2D collider )
    {
        if ( mIsInvincible )
        {

        }
        else
        {
            //ParticleSystemManager.Instance.CreatePlayerExplision( Position );
        }
    }

    public float GetY()
    {
        return mTransform.position.y;
    }

    public void FlashInvincibility()
    {
        mIsInvincible = true;
        mInvincibilityRemaining = mFlashInvincibilityDuration;
    }
}
