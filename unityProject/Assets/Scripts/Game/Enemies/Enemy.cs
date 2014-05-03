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
        Debug.Log( "destroy enemy" );
        //Messenger.Broadcast<int>( Events.GameEvents.IncrementScore, 10 );
        ParticleSystemManager.Instance.CreateEnemyExplosion( Position );
        Recycle();
    }

    public virtual void Recycle()
    {
        Debug.Log( "base Recycle" );
    }
}
