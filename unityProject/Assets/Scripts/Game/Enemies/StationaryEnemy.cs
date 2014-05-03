using UnityEngine;
using System.Collections;

public class StationaryEnemy : Enemy
{
    private float mEasingAmount = 0.15f;

    void Start()
    {

    }

    void Update()
    {
        /*
        Vector2 target = -PlayerSnake.Instance.Position - Position;

        Velocity = new Vector2( target.x * mEasingAmount, 0 );

        Position += Velocity;
         * */
    }

    public override void CollisionTriggered( Collider2D collider )
    {
        /*
        switch ( collider.tag )
        {
            case "Player":
                ParticleSystemManager.Instance.CreateEnemyExplosion( Position );
                ObjectPool.Recycle( this );
                break;
        }*/
    }

    public override void Recycle()
    {
        Debug.Log( "Recycle" );
    }

    public void DestroyThisEnemy()
    {
        Debug.Log( "recycle this" );
        Messenger.Broadcast<int>( Events.GameEvents.IncrementScore, 10 );
        ParticleSystemManager.Instance.CreateEnemyExplosion( Position );
        ObjectPool.Recycle( this );
    }
}
