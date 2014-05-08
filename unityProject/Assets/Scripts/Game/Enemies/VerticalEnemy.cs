using UnityEngine;
using System.Collections;

public class VerticalEnemy : Enemy
{
    public Transform positionTarget;
    public float movementDuration = 2.0f;
    public float rotationDuration = 0.5f;

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

        Rotate();
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        Go.killAllTweensWithTarget( transform );
    }

    void OnCompleteMove( AbstractGoTween tween )
    {
        mMovingTowardsPosition2 = !mMovingTowardsPosition2;

        Rotate();
    }

    void OnCompleteRotate( AbstractGoTween tween )
    {
        Move();
    }

    void Rotate()
    {
        Quaternion newRot = Quaternion.identity;
        if ( mMovingTowardsPosition2 )
        {
            newRot = Quaternion.Euler( new Vector3( 0, 0, -180 ) );
        }
        else
        {
            transform.localRotation = Quaternion.Euler( new Vector3( 0, 0, 180 ) );
        }

        Go.to( transform, rotationDuration, new GoTweenConfig().localRotation( newRot ) ).setOnCompleteHandler( OnCompleteRotate );
    }

    void Move()
    {

        Vector3 newPos;
        if ( mMovingTowardsPosition2 )
        {
            newPos = new Vector3( mPosition2.x, mPosition2.y, gameObject.transform.position.z );
        }
        else
        {
            newPos = new Vector3( mPosition1.x, mPosition1.y, gameObject.transform.position.z );
        }

        Go.to( transform, movementDuration, new GoTweenConfig().position( newPos ) ).setOnCompleteHandler( OnCompleteMove );
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
