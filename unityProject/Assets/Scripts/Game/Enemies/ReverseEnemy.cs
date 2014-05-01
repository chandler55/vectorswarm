using UnityEngine;
using System.Collections;

public class ReverseEnemy : Entity
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

    public override void CollisionTriggered( Collider2D collider )
    {
        ParticleSystemManager.Instance.CreateEnemyExplosion( Position );
        ObjectPool.Recycle( this );
    }
}
