using UnityEngine;
using System.Collections;

public class PlayerSnake : Entity
{
    public GameObject shipObject = null;
    public tk2dSprite playerSprite = null;
    public tk2dRadialSprite shieldSprite = null;

    private static PlayerSnake instance;
    public static PlayerSnake Instance
    {
        get
        {
            return instance;
        }
    }

    public CircleCollider2D deathCollider;
    public CircleCollider2D destroyEnemyCollider;

    // speed
    private float mEasingAmount = 9.0f;
    private float mPlayerNormalSpeed = 15.0f;
    private float mCurrentPlayerSpeedY = 15.0f;
    private float mPlayerAfterburnerSpeed = 30.0f;

    private Transform mTransform = null;

    // fuel
    private float   mFuelCapacity = 6.0f; // in seconds
    private float   mFuelRemaining = 0.0f;
    private bool    mAfterburnerActivated = false;
    private float   mFuelChargeRate = 0.1f;

    //invincibility
    private float   mFlashInvincibilityDuration = 2.5f;
    private float   mInvincibilityRemaining = 0.0f;

    // alive
    private bool    mIsAlive = true;

    // controls
    private float   mPreviousAccelerationX = 0.0f;
    private float   mThresholdForAccelerometer = 0.01f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameUtils.Assert( shieldSprite );
        GameUtils.Assert( playerSprite );
        mTransform = gameObject.transform;
        mFuelRemaining = mFuelCapacity;

        Messenger.AddListener( Events.GameEvents.SpawnNewShip, OnSpawnNewShip );

        FlashInvincibility();
    }

    void OnDestroy()
    {
        instance = null;

        Messenger.RemoveListener( Events.GameEvents.SpawnNewShip, OnSpawnNewShip );
    }

    void Update()
    {
        if ( !mIsAlive )
        {
            return;
        }

        if ( Input.GetKey( KeyCode.X ) )
        {
            SetPlayerSpeed( mPlayerAfterburnerSpeed );
        }
        else if ( Input.GetKey( KeyCode.C ) )
        {
            SetPlayerSpeed( 0 );
            //SetPlayerSpeed( mPlayerNormalSpeed );
        }

        if ( deathCollider && destroyEnemyCollider )
        {
            deathCollider.enabled = !mAfterburnerActivated;
            destroyEnemyCollider.enabled = mAfterburnerActivated;
        }

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
                playerSprite.color = Color.white;
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
            SetAfterburner( true );
            SetPlayerSpeed( mPlayerAfterburnerSpeed );
        }

        if ( mAfterburnerActivated )
        {
            mFuelRemaining -= Time.deltaTime;
            if ( mFuelRemaining < 0 )
            {
                mFuelRemaining = 0;
                SetAfterburner( false );
                SetPlayerSpeed( mPlayerNormalSpeed );

                // give player temporary invincibility
                FlashInvincibility();

                // shoot a clear screen bomb
                EntityDatabase.Instance.CreateEntity( EntityDatabase.EntityType.EntityType_ScreenBomb, transform.position, Quaternion.identity );
            }
            Messenger.Broadcast<float>( Events.UIEvents.FuelGaugeUpdated, mFuelRemaining / mFuelCapacity );
        }
        else
        {
            if ( mFuelRemaining <= mFuelCapacity )
            {
                mFuelRemaining += Time.deltaTime * mFuelChargeRate * mFuelCapacity;

                if ( mFuelRemaining > mFuelCapacity )
                {
                    mFuelRemaining = mFuelCapacity;
                }

                Messenger.Broadcast<float>( Events.UIEvents.FuelGaugeUpdated, mFuelRemaining / mFuelCapacity );
            }
        }
    }

    private void SetPlayerSpeed( float speed )
    {
        mCurrentPlayerSpeedY = speed;
        Messenger.Broadcast<float>( Events.GameEvents.PlayerSpeedUpdated, mCurrentPlayerSpeedY );
    }

    private void SetAfterburner( bool on )
    {
        if ( shieldSprite )
        {
            shieldSprite.color = on ? Color.white : Color.clear;
        }

        mAfterburnerActivated = on;
        Messenger.Broadcast<bool>( Events.GameEvents.AfterburnerTriggered, on );
    }

    private void MovementLogic()
    {
        //Player Horizontal Movement
        {
#if UNITY_ANDROID || UNITY_IPHONE

            // move towards accelerometer tilt
            // tilt to horizontal percentage -0.25 to 0.25
            float accelerationValueToUse = 0.0f;

            if ( mPreviousAccelerationX - Input.acceleration.x < mThresholdForAccelerometer )
            {
                accelerationValueToUse = mPreviousAccelerationX;
            }
            else
            {
                accelerationValueToUse = Input.acceleration.x;
            }

            float percentage = Mathf.InverseLerp( -0.25f, 0.25f, accelerationValueToUse );
            mPreviousAccelerationX = Input.acceleration.x;

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
        if ( mAfterburnerActivated || Input.GetKey( KeyCode.X ) )
        {
            if ( collider.tag == "Enemy" )
            {
                GameObject go = collider.gameObject;
                go.SendMessage( "DestroyEnemy" );
            }
        }
        else if ( mInvincibilityRemaining > 0.0f )
        {

        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        // if shield kills all enemies on screen
        //if ( PlayerProfile.GetInstance().hasKillAllShield )

        mIsAlive = false;

        SetPlayerSpeed( 0.0f );

        ParticleSystemManager.Instance.CreatePlayerExplosion( Position );

        Messenger.Broadcast( Events.GameEvents.PlayerDied );

        if ( shipObject != null )
        {
            shipObject.SetActive( false );
        }

        /*
        PlayerShield shield = GetComponentInChildren<PlayerShield>();
        if ( shield )
        {
            bool shieldCharged = shield.IsCharged();

            if ( shieldCharged )
            {
                shield.UseShield();
            }
            else
            {
                
            }
        }*/
    }

    private void Spawn()
    {
        mIsAlive = true;

        if ( shipObject != null )
        {
            shipObject.SetActive( true );
        }

        FlashInvincibility();

        SetPlayerSpeed( mPlayerNormalSpeed );
    }

    void OnSpawnNewShip()
    {
        Spawn();
    }
}
