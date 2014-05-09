using UnityEngine;
using System.Collections;

public class SineEnemy : Enemy
{
    public float waveHeight = 6.0f;

    private bool mMovingRight = false;
    private const float SPEED = 30.0f;
    private float mBaselineY = 0.0f;
    private tk2dSprite mSprite = null;

    protected override void Init()
    {
        base.Init();

        mMovingRight = Position.x > 0;
        if ( mSprite )
        {
            mSprite.FlipX = !mMovingRight;
        }

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
                if ( mSprite )
                {
                    mSprite.FlipX = true;
                }
            }
        }
        else
        {
            if ( Position.x <= GameSettings.WORLD_BOUNDARY.x )
            {
                mMovingRight = true;
                if ( mSprite )
                {
                    mSprite.FlipX = false;
                }
            }
        }

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

        Velocity = Velocity.normalized * SPEED;

        float timeSinceStartup = Time.realtimeSinceStartup;
        Velocity = new Vector2( Velocity.x, ( ( mBaselineY - Position.y ) + Mathf.Sin( timeSinceStartup * 10.0f ) * waveHeight ) );

        Position += Velocity * Time.deltaTime;

        //Go.to( transform, movementDuration, new GoTweenConfig().position( newPos ) ).setOnCompleteHandler( OnCompleteTween );
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
