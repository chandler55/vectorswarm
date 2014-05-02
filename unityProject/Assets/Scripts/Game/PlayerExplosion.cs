using UnityEngine;
using System.Collections;

public class PlayerExplosion : Entity
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

    public void SetColor( Color c )
    {
        if ( mParticleSystem )
        {
            mParticleSystem.startColor = c;
        }
    }
}
