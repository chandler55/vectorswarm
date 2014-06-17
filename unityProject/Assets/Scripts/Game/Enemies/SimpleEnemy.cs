using UnityEngine;
using System.Collections;

// this enemy moves left and right , nothing special
public class SimpleEnemy : Enemy
{
    public float movementSpeed = 2.0f;

    private bool mMovingRight = false;

    protected override void Init()
    {
        base.Init();
        mMovingRight = Position.x > 0;
    }

    public void SetMovingRight( bool moveRight )
    {
        mMovingRight = moveRight;
    }

    void Update()
    {
        if ( mMovingRight )
        {
            if ( Position.x >= GameSettings.WORLD_BOUNDARY.x + GameSettings.WORLD_BOUNDARY.width )
            {
                mMovingRight = false;
            }
        }
        else
        {
            if ( Position.x <= GameSettings.WORLD_BOUNDARY.x )
            {
                mMovingRight = true;
            }
        }

        if ( mMovingRight )
        {
            Velocity = new Vector2( movementSpeed, 0 );
        }
        else
        {
            Velocity = new Vector2( -movementSpeed, 0 );
        }

        Position += Velocity * Time.deltaTime;
    }

    void Move()
    {
        mMovingRight = !mMovingRight;
    }

    public override void DestroyEnemy()
    {
        base.DestroyEnemy();
        Die();
    }

    public override void Die()
    {
        base.Die();
        ObjectPool.Recycle( this );
    }
}
