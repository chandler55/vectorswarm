using UnityEngine;
using System.Collections;

public class PlayerSnake : Entity
{
    public bool invulnerableToggle = false;
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

    public Transform gunTransform;

    // speed
    private float mPlayerNormalSpeed = 20.0f;
    private float mCurrentPlayerSpeed = 20.0f;
    private float mPlayerAfterburnerSpeed = 30.0f;

    private Transform mTransform = null;

    // fuel
    private float   mFuelCapacity = 4.0f; // in seconds
    private float   mFuelRemaining = 0.0f;
    private bool    mAfterburnerActivated = false;
    private float   mFuelChargeRate = 0.08f;

    //invincibility
    private float   mFlashInvincibilityDuration = 2.5f;
    private float   mInvincibilityRemaining = 0.0f;

    // alive
    private bool    mIsAlive = true;

    // respawning
    private Vector3 mStartPosition = Vector3.zero;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameUtils.Assert( gunTransform );
        GameUtils.Assert( shieldSprite );
        GameUtils.Assert( playerSprite );

        mStartPosition = gameObject.transform.position;
        mTransform = gameObject.transform;

        Messenger.AddListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
        Messenger.AddListener( Events.GameEvents.GameStart, OnGameStarted );
        Messenger.AddListener( Events.GameEvents.TriggerAfterburner, OnTriggerAfterburner );

        // start with player disabled

        //DisablePlayer();

        //FlashInvincibility();
    }

    void OnDestroy()
    {
        instance = null;

        Messenger.RemoveListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
        Messenger.RemoveListener( Events.GameEvents.GameStart, OnGameStarted );
        Messenger.RemoveListener( Events.GameEvents.TriggerAfterburner, OnTriggerAfterburner );
    }

    void Update()
    {
        if ( !mIsAlive )
        {
            return;
        }

#if DEBUG
        if ( Input.GetKey( KeyCode.X ) && !mAfterburnerActivated )
        {
            Messenger.Broadcast( Events.GameEvents.TriggerAfterburner );
            // SetPlayerSpeed( mPlayerAfterburnerSpeed );
        }
        else if ( Input.GetKey( KeyCode.C ) )
        {
            SetPlayerSpeed( 0 );
            //SetPlayerSpeed( mPlayerNormalSpeed );
        }
#endif

        if ( deathCollider && destroyEnemyCollider )
        {
            deathCollider.enabled = !mAfterburnerActivated;
            destroyEnemyCollider.enabled = mAfterburnerActivated;
        }

        // invincibility logic
        InvincibilityLogic();

        // Afterburner Logic
        AfterburnerLogic();

        Position += Velocity * Time.deltaTime;
        Position = Playfield.Instance.ClampBoundary( Position );
        Messenger.Broadcast( Events.GameEvents.PlayerMoved );
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

    void OnTriggerAfterburner()
    {
        SoundManager.Instance.PlaySound( SoundManager.Sounds.Sounds_Afterburner );
        SetAfterburner( true );
        SetPlayerSpeed( mPlayerAfterburnerSpeed );
    }

    private void SetPlayerSpeed( float speed )
    {
        mCurrentPlayerSpeed = speed;
        Messenger.Broadcast<float>( Events.GameEvents.PlayerSpeedUpdated, mCurrentPlayerSpeed );
    }

    private void SetAfterburner( bool on )
    {
        if ( shieldSprite )
        {
            shieldSprite.color = on ? Color.white : Color.clear;
        }

        mAfterburnerActivated = on;

        if ( on )
        {
            Go.to( shipObject.transform, 0.3f, new GoTweenConfig().scale( new Vector3( 1.5f, 1.5f, 1.0f ) ) );
        }
        else
        {
            Go.to( shipObject.transform, 0.4f, new GoTweenConfig().scale( Vector3.one ) );
        }

        Messenger.Broadcast<bool>( Events.GameEvents.AfterburnerTriggered, on );
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
        if ( mAfterburnerActivated )
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
            if ( collider.tag == "Enemy" )
            {
                if ( !invulnerableToggle )
                {
                    Die();
                }
            }
        }
    }

    public override void Die()
    {
        DisablePlayer();

        SoundManager.Instance.PlaySound( SoundManager.Sounds.Sounds_PlayerHit );

        ParticleSystemManager.Instance.CreatePlayerExplosion( Position );

        Messenger.Broadcast( Events.GameEvents.PlayerDied );
    }

    private void Spawn()
    {
        mFuelRemaining = mFuelCapacity;

        EnablePlayer();

        FlashInvincibility();
    }

    void OnNewGameStarted()
    {
        gameObject.transform.position = mStartPosition;
        Spawn();

        Messenger.Broadcast( Events.GameEvents.PlayerMoved );
    }

    void OnGameStarted()
    {
        SetPlayerSpeed( mPlayerNormalSpeed );
    }

    void DisablePlayer()
    {
        mIsAlive = false;

        SetPlayerSpeed( 0.0f );

        if ( shipObject != null )
        {
            shipObject.SetActive( false );
        }
    }

    void EnablePlayer()
    {
        mIsAlive = true;

        if ( shipObject != null )
        {
            shipObject.SetActive( true );
        }

        SetPlayerSpeed( mPlayerNormalSpeed );
    }

    public void Move( Vector2 direction )
    {
        Velocity = direction * mCurrentPlayerSpeed;
        if ( direction.sqrMagnitude > 0.1f )
        {
            UpdateRotation();
        }
    }
}
