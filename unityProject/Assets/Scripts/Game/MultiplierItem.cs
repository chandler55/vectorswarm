using UnityEngine;
using System.Collections;

public class MultiplierItem : Entity
{
    private const float MULTIPLIER_GRAB_DISTANCE = 7.0f;
    private const float CHASE_SPEED = 30.0f;

    private tk2dSprite mSprite = null;

    // stretching animation
    private bool    mShrinkingX = false;
    private Vector3 mOriginalScale;

    private bool    mHeadTowardsPlayerShip = false;

    void Start()
    {
        mSprite = GetComponentInChildren<tk2dSprite>();

        if ( mSprite )
        {
            mOriginalScale = mSprite.transform.localScale;
            OnCompleteSquish( null );
        }
    }

    void Update()
    {
        // head towards player ship
        if ( PlayerSnake.Instance )
        {
            Vector2 mPlayerPos = PlayerSnake.Instance.Position;
            float distance = ( Position - mPlayerPos ).magnitude;

            if ( mHeadTowardsPlayerShip )
            {
                if ( distance < 1.0f )
                {
                    Messenger.Broadcast<long>( Events.GameEvents.IncrementScore, 10 );
                    Messenger.Broadcast<Vector3>( Events.GameEvents.IncrementMultipler, Position );
                    SoundManager.Instance.PlaySound( SoundManager.Sounds.Sounds_GetMultiplier );
                    ObjectPool.Recycle( this );
                }
                else
                {
                    Velocity = ( mPlayerPos - Position ).normalized * CHASE_SPEED;
                    Position += Velocity * Time.deltaTime;
                }
            }
            else
            {
                if ( distance < MULTIPLIER_GRAB_DISTANCE )
                {
                    mHeadTowardsPlayerShip = true;
                }
            }
        }
    }

    void OnDisable()
    {
        if ( mSprite )
        {
            Go.killAllTweensWithTarget( mSprite.transform );
        }
    }

    void OnCompleteSquish( AbstractGoTween tween )
    {
        mShrinkingX = !mShrinkingX;
        Vector3 squishScale;
        if ( mShrinkingX )
        {
            squishScale = new Vector3( mOriginalScale.x * 0.55f, mOriginalScale.y * 1.5f, mOriginalScale.z );
        }
        else
        {
            squishScale = new Vector3( mOriginalScale.x * 1.5f, mOriginalScale.y * 0.55f, mOriginalScale.z );
        }

        Go.to( mSprite.transform, 0.35f, new GoTweenConfig().scale( squishScale ).setEaseType( GoEaseType.Linear ).setIterations( 2, GoLoopType.PingPong ).onComplete( OnCompleteSquish ) );
    }

    public override void Reset()
    {
        base.Reset();
        mHeadTowardsPlayerShip = false;
        Velocity = Vector2.zero;
    }

    public override void CollisionTriggered( Collider2D collider )
    {

    }

    public void Die()
    {
        ObjectPool.Recycle( this );
    }
}
