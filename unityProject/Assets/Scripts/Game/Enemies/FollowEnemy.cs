using UnityEngine;
using System.Collections;

public class FollowEnemy : Enemy
{
    private float mEasingAmount = 0.15f;

    void Start()
    {

    }

    void Update()
    {
        Vector2 target = PlayerSnake.Instance.Position - Position;
        Velocity = new Vector2( target.x * mEasingAmount, 0 ) / 8.0f;
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
