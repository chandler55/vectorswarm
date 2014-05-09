using UnityEngine;
using System.Collections;

public class StationaryEnemy : Enemy
{
    private Vector2 mOriginalPosition = Vector2.zero;

    void OnDisable()
    {
        Go.killAllTweensWithTarget( transform );
    }

    protected override void Init()
    {
        mOriginalPosition = Position;

        MoveToNewPosition( null );
    }

    void MoveToNewPosition( AbstractGoTween tween )
    {
        Vector2 newPos = new Vector2( mOriginalPosition.x + Random.Range( -1.0f, 1.0f ), mOriginalPosition.y + Random.Range( -1.0f, 1.0f ) );
        Go.to( transform, 1.0f, new GoTweenConfig().position( newPos ) ).setOnCompleteHandler( MoveToNewPosition );
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
