using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GatesDynamite : Entity
{

    void Start()
    {

    }

    void Update()
    {

    }

    void CollisionTriggered( Collider2D collider )
    {
        // destroy any birds
        List<Bird> birdsList = BirdManager.Instance.GetBirdsList();
        foreach ( Bird b in birdsList )
        {
            Vector2 vectorDiff = b.Position - Position;
            if ( vectorDiff.sqrMagnitude < 100 )
            {
                b.Destroy();
            }
        }

        ParticleSystemManager.Instance.CreatePlayerExplision( Position );
        //Destroy( gameObject );
    }
}
