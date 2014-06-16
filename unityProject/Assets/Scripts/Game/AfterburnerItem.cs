using UnityEngine;
using System.Collections;

public class AfterburnerItem : Entity
{
    private const float AFTERBURNER_GRAB_DISTANCE = 4.0f;
    private const float CHASE_SPEED = 60.0f;

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
                if ( distance < 2.0f )
                {
                    Messenger.Broadcast( Events.GameEvents.TriggerAfterburner );
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
                if ( distance < AFTERBURNER_GRAB_DISTANCE )
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

    public override void Die()
    {
        base.Die();
        ObjectPool.Recycle( this );
    }
}
