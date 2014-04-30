using UnityEngine;
using System.Collections;

public class PlayerSnake : Entity
{
    public tk2dSprite playerSprite = null;

    private static PlayerSnake instance;
    public static PlayerSnake Instance
    {
        get
        {
            return instance;
        }
    }

    // speed
    private float mEasingAmount = 9.0f;
    private float mPlayerNormalSpeed = 15.0f;
    private float mCurrentPlayerSpeedY = 15.0f;
    private float mPlayerAfterburnerSpeed = 60.0f;

    private Transform mTransform = null;

    // fuel
    private float   mFuelCapacity = 1.0f; // in seconds
    private float   mFuelRemaining = 0.0f;
    private bool    mAfterburnerActivated = false;
    private float   mFuelChargeRate = 0.1f;

    //invincibility
    private float   mFlashInvincibilityDuration = 1.5f;
    private float   mInvincibilityRemaining = 0.0f;

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
        InvincibilityLogic();

        // Afterburner Logic
        AfterburnerLogic();

        // movement logic
        MovementLogic();

        // lock y velocity to mCurrentPlayerSpeedY variable
        Velocity = new Vector2( Velocity.x, mCurrentPlayerSpeedY );
        Position += Velocity * Time.deltaTime;
    }

    private void InvincibilityLogic()
    {
        if ( mInvincibilityRemaining > 0 )
        {

            mInvincibilityRemaining -= Time.deltaTime;
            if ( mInvincibilityRemaining <= 0 )
            {
                mInvincibilityRemaining = 0.0f;
            }
            else
            {
                if ( playerSprite )
                {
                    Color newColor = playerSprite.color;
                    newColor.a = Mathf.Abs( Mathf.Cos( mInvincibilityRemaining * GameSettings.INVINCIBILITY_FLASH_SPEED ) );
                    playerSprite.color = newColor;
                }
            }
        }
    }

    private void AfterburnerLogic()
    {
        if ( Input.GetMouseButton( 0 ) && mFuelRemaining == mFuelCapacity )
        {
            mAfterburnerActivated = true;
            SetPlayerSpeed( mPlayerAfterburnerSpeed );
        }

        if ( mAfterburnerActivated )
        {
            mFuelRemaining -= Time.deltaTime;
            if ( mFuelRemaining < 0 )
            {
                mFuelRemaining = 0;
                mAfterburnerActivated = false;
                SetPlayerSpeed( mPlayerNormalSpeed );

                // give player temporary invincibility
                FlashInvincibility();

            }

            Messenger.Broadcast<float>( Events.UIEvents.FuelGaugeUpdated, mFuelRemaining / mFuelCapacity );
        }
        else
        {
            if ( mFuelRemaining <= mFuelCapacity )
            {
                mFuelRemaining += Time.deltaTime * mFuelChargeRate;

                if ( mFuelRemaining > mFuelCapacity )
                {
                    mFuelRemaining = mFuelCapacity;
                }

                Messenger.Broadcast<float>( Events.UIEvents.FuelGaugeUpdated, mFuelRemaining / mFuelCapacity );
            }
        }
    }
    
    private void SetPlayerSpeed(float speed)
    {
        mCurrentPlayerSpeedY = speed;
        Messenger.Broadcast<float>( Events.GameEvents.PlayerSpeedUpdated, mCurrentPlayerSpeedY );
    }

    private void MovementLogic()
    {
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
    }

    public float GetY()
    {
        return mTransform.position.y;
    }

    public void FlashInvincibility()
    {
        mInvincibilityRemaining = mFlashInvincibilityDuration;
    }

    public override void CollisionTriggered( Collider2D collider )
    {
        if ( mInvincibilityRemaining > 0.0f )
        {

        }
        else
        {
            //ParticleSystemManager.Instance.CreatePlayerExplision( Position );
        }
    }
}
