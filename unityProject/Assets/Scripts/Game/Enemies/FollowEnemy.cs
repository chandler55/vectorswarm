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

    public override void CollisionTriggered( Collider2D collider )
    {
        base.CollisionTriggered( collider );
    }

    public override void Recycle()
    {
        Debug.Log( "Recycle" );
        ObjectPool.Recycle( this );
    }
}
