using UnityEngine;
using System.Collections;

public class LeverEnemy : Enemy
{
    private float mEasingAmount = 9.0f;
    private Transform mTransform = null;

    void Start()
    {
        mTransform = transform;
    }

    void Update()
    {
        //Vector2 target = -PlayerSnake.Instance.Position - Position;
        //Velocity = new Vector2( target.x * mEasingAmount, 0 );
        //Position += Velocity * Time.deltaTime;

        float yDistanceToPlayer = PlayerSnake.Instance.Position.y - Position.y;
        if ( mTransform )
        {
            mTransform.localRotation = Quaternion.Euler(new Vector3( 0, 0, yDistanceToPlayer + 90 ));
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
