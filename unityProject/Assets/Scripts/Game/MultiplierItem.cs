using UnityEngine;
using System.Collections;

public class MultiplierItem : Entity
{
    private const float MULTIPLIER_GRAB_DISTANCE = 7.0f;
    private const float CHASE_SPEED = 30.0f;

    private bool    mHeadTowardsPlayerShip = false;

    void Start()
    {
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
                    Messenger.Broadcast<long>( Events.GameEvents.IncrementScore, 1 );
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

    public override void Reset()
    {
        base.Reset();
        mHeadTowardsPlayerShip = false;
        Velocity = Vector2.zero;
    }

    public override void CollisionTriggered( Collider2D collider )
    {

    }

    public override void Die()
    {
        base.Die();
        ObjectPool.Recycle( this );
    }
}
