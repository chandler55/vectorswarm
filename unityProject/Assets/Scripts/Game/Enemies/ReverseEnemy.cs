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
