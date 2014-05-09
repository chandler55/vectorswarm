using UnityEngine;
using System.Collections;

public class LeverEnemy : Enemy
{
    private Transform mTransform = null;

    protected override void Init()
    {
        base.Init();

        mTransform = transform;
    }

    void Update()
    {
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
