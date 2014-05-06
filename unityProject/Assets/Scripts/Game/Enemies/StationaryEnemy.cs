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
        Messenger.Broadcast<int>( Events.GameEvents.IncrementScore, 10 );
        ParticleSystemManager.Instance.CreateEnemyExplosion( Position );
        ObjectPool.Recycle( this );
    }
}
