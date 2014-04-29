using UnityEngine;
using System.Collections;

public class FollowEnemy : Entity
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

    public override void CollisionTriggered( Collider2D collider )
    {
        ParticleSystemManager.Instance.CreateEnemyExplosion( Position );
        Destroy( gameObject );
    }
}
