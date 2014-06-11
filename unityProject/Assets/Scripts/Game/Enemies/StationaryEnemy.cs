using UnityEngine;
using System.Collections;

public class StationaryEnemy : Enemy
{
    public float randomSpread = 0.7f;
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
        Vector2 newPos = new Vector2( mOriginalPosition.x + Random.Range( -randomSpread, randomSpread ), mOriginalPosition.y + Random.Range( -randomSpread, randomSpread ) );
        Go.to( transform, 1.0f, new GoTweenConfig().position( newPos ) ).setOnCompleteHandler( MoveToNewPosition );
    }

    public override void DestroyEnemy()
    {
        base.DestroyEnemy();
        Die();
    }

    public override void Die()
    {
        base.Die();
        ObjectPool.Recycle( this );
    }
}
