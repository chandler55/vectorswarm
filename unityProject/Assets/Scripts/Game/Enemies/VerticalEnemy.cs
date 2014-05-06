using UnityEngine;
using System.Collections;

public class VerticalEnemy : Enemy
{
    public Transform positionTarget;
    public float movementDuration = 2.0f;

    private bool mMovingTowardsPosition2 = false;
    private Vector3 mPosition1 = Vector3.zero;
    private Vector3 mPosition2 = Vector3.zero;

    void Start()
    {
        mPosition1 = Position;
        if ( positionTarget )
        {
            mPosition2 = positionTarget.position;
        }
        
        mMovingTowardsPosition2 = true;

        Move();
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        Go.killAllTweensWithTarget( transform );
    }

    void OnCompleteTween( AbstractGoTween tween )
    {
        Move();
    }

    void Move()
    {
        mMovingTowardsPosition2 = !mMovingTowardsPosition2;

        Vector3 newPos;
        if ( mMovingTowardsPosition2 )
        {
            newPos = new Vector3( mPosition2.x, mPosition2.y, gameObject.transform.position.z );
        }
        else
        {
            newPos = new Vector3( mPosition1.x, mPosition1.y, gameObject.transform.position.z );
        }

        Go.to( transform, movementDuration, new GoTweenConfig().position( newPos ) ).setOnCompleteHandler( OnCompleteTween );
    }

    public override void DestroyEnemy()
    {
        base.DestroyEnemy();
        Die();
    }

    public void Die()
    {
        Messenger.Broadcast<int>( Events.GameEvents.IncrementScore, 10 );
        ParticleSystemManager.Instance.CreateEnemyExplosion( Position );
        ObjectPool.Recycle( this );
    }
}
