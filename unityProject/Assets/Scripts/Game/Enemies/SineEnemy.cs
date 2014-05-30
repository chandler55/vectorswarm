using UnityEngine;
using System.Collections;

public class SineEnemy : Enemy
{
    public float speed = 25.0f;

    public float waveHeight = 6.0f;

    protected float mBaselineY = 0.0f;
    private bool mMovingRight = false;
    private tk2dSprite mSprite = null;

    protected override void Init()
    {
        base.Init();

        mMovingRight = Position.x > 0;
        FixOrientation();

        mBaselineY = Position.y;
        mSprite = GetComponent<tk2dSprite>();

    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if ( mMovingRight )
        {
            if ( Position.x >= GameSettings.WORLD_BOUNDARY.x + GameSettings.WORLD_BOUNDARY.width )
            {
                mMovingRight = false;
                FixOrientation();
            }
        }
        else
        {
            if ( Position.x <= GameSettings.WORLD_BOUNDARY.x )
            {
                mMovingRight = true;
                FixOrientation();
            }
        }

        // calculate velocity
        Vector3 newPos;
        if ( mMovingRight )
        {
            newPos = new Vector3( GameSettings.WORLD_BOUNDARY.x + GameSettings.WORLD_BOUNDARY.width, Position.y, gameObject.transform.position.z );
        }
        else
        {
            newPos = new Vector3( GameSettings.WORLD_BOUNDARY.x, Position.y, gameObject.transform.position.z );
        }

        Velocity = (Vector2)newPos - Position;

        Velocity = Velocity.normalized * speed;

        CalculateVelocityY();
        Position += Velocity * Time.deltaTime;

        //Go.to( transform, movementDuration, new GoTweenConfig().position( newPos ) ).setOnCompleteHandler( OnCompleteTween );
    }

    protected virtual void CalculateVelocityY()
    {
        float timeSinceStartup = Time.realtimeSinceStartup;
        Velocity = new Vector2( Velocity.x, ( ( mBaselineY - Position.y ) + Mathf.Sin( timeSinceStartup * 10.0f ) * waveHeight ) );
    }

    void FixOrientation()
    {
        if ( mSprite )
        {
            if ( !mMovingRight )
            {
                gameObject.transform.localScale = new Vector3( -1, 1, 1 );
                if ( gameObject.transform.lossyScale.x != -1 )
                {
                    gameObject.transform.localScale = new Vector3( 1, 1, 1 );
                }
            }
            else
            {
                gameObject.transform.localScale = new Vector3( 1, 1, 1 );
                if ( gameObject.transform.lossyScale.x != 1 )
                {
                    gameObject.transform.localScale = new Vector3( -1, 1, 1 );
                }
            }
        }
    }

    public override void DestroyEnemy()
    {
        base.DestroyEnemy();
        Die();
    }

    public void Die()
    {
        ObjectPool.Recycle( this );
    }
}
