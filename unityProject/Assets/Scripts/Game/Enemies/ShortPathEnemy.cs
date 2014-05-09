using UnityEngine;
using System.Collections;

public class ShortPathEnemy : Enemy
{
    public float movementDuration = 2.0f;
    public float movementDistanceBeforePause = 1.0f;

    private bool mMovingRight = false;

    private float mPauseDuration = 0.5f;

    protected override void Init()
    {
        base.Init();
        mMovingRight = Position.x > 0;
        Move();
    }

    void OnDisable()
    {
        Go.killAllTweensWithTarget( transform );
    }

    void OnCompleteMove( AbstractGoTween tween )
    {
        Go.to( transform, mPauseDuration, new GoTweenConfig() ).setOnCompleteHandler( OnCompletePause );
    }

    void OnCompletePause( AbstractGoTween tween )
    {
        Move();
    }

    void Move()
    {

        Vector3 newPos;
        if ( mMovingRight )
        {
            float newX = Position.x + movementDistanceBeforePause;
            if ( newX >= GameSettings.WORLD_BOUNDARY.x + GameSettings.WORLD_BOUNDARY.width )
            {
                newX = GameSettings.WORLD_BOUNDARY.x + GameSettings.WORLD_BOUNDARY.width;
                mMovingRight = false;
            }

            newPos = new Vector3( newX, Position.y, gameObject.transform.position.z );
        }
        else
        {
            float newX = Position.x - movementDistanceBeforePause;
            if ( newX <= GameSettings.WORLD_BOUNDARY.x )
            {
                newX = GameSettings.WORLD_BOUNDARY.x;
                mMovingRight = true;
            }

            newPos = new Vector3( newX, Position.y, gameObject.transform.position.z );
        }

        Go.to( transform, 0.1f, new GoTweenConfig().position( newPos ) ).setOnCompleteHandler( OnCompleteMove );
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

