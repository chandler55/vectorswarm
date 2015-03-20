using UnityEngine;
using System.Collections;

public class BulletExplosion : Entity
{
    private ParticleSystem mParticleSystem;

    void Start()
    {
        mParticleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if ( mParticleSystem )
        {
            if ( !mParticleSystem.IsAlive() )
            {
                ObjectPool.Recycle( this );
            }
        }
    }
}
