using UnityEngine;
using System.Collections;

public class ReverseEnemy : Enemy
{
    private float mEasingAmount = 9.0f;

    void Start()
    {

    }

    void Update()
    {
        Vector2 target = -PlayerSnake.Instance.Position - Position;
        Velocity = new Vector2( target.x * mEasingAmount, 0 );
        Position += Velocity * Time.deltaTime;
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
