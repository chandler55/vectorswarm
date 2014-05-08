using UnityEngine;
using System.Collections;

public class StationaryEnemy : Enemy
{
    private float mEasingAmount = 0.15f;

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
