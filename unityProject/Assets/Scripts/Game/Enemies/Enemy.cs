using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Entity
{
    public override void CollisionTriggered( Collider2D collider )
    {
        base.CollisionTriggered( collider );
        switch ( collider.tag )
        {
            case "Player":
                break;
        }
    }

    public virtual void DestroyEnemy()
    {
        Messenger.Broadcast<int>( Events.GameEvents.IncrementScore, 10 );
        ParticleSystemManager.Instance.CreateEnemyExplosion( Position );

        // spawn multiplier item
        EntityDatabase.Instance.CreateEntity( EntityDatabase.EntityType.EntityType_MultiplierItem, Position, Quaternion.identity );

        //Recycle();
    }

    public virtual void Recycle()
    {
        //Debug.Log( "base Recycle" );
    }
}
