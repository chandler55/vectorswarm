using UnityEngine;
using System.Collections;

public class HourGlassEnemy : Enemy
{
    public Transform    originOfCircle = null;

    public float        movementSpeed = 2.0f;

    private float mAngle = 0.0f;
    private float mRadius = 180.0f;
    private Vector3 mOriginPos = Vector3.zero;

    protected override void Init()
    {
        mRadius = ( Position - (Vector2)originOfCircle.position ).magnitude;
        mOriginPos = originOfCircle.position;
    }

    void Update()
    {
        if ( originOfCircle )
        {
            mAngle -= Time.deltaTime * movementSpeed;
            Vector2 newPos = new Vector2( Mathf.Cos( mAngle ) * mRadius + mOriginPos.x, Mathf.Sin( mAngle ) * mRadius + mOriginPos.y );
            Position = newPos;
        }
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
