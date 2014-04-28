using UnityEngine;
using System.Collections;

public class ReverseEnemy : Entity
{
    private float mEasingAmount = 0.15f;

    void Start()
    {

    }

    void Update()
    {
        Vector2 target = PlayerSnake.Instance.Position - Position;

        Velocity = new Vector2( target.x * mEasingAmount, 0 ) / 8.0f;

        Position += Velocity;
    }

    public override void CollisionTriggered( Collider2D collider )
    {
        ParticleSystemManager.Instance.CreateEnemyExplosion( Position );
        Destroy( gameObject );
    }
}
