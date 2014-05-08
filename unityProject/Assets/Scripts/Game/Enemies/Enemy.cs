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
        Messenger.Broadcast<long, Vector3>( Events.GameEvents.IncrementScore, 10, Position );
        ParticleSystemManager.Instance.CreateEnemyExplosion( Position );
        

        //Recycle();
    }

    public virtual void Recycle()
    {
        //Debug.Log( "base Recycle" );
    }
}
