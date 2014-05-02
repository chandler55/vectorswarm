using UnityEngine;
using System.Collections;

public class ExplosionParticles : Entity
{
    private ParticleSystem mParticleSystem;

    void Start()
    {
        mParticleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.H ) )
        {
            if ( mParticleSystem )
            {
                mParticleSystem.Stop();
                mParticleSystem.startColor = Color.yellow;
                mParticleSystem.Play();

                /*
                mParticleSystem.GetParticles( particles );
                for ( int i = 0; i < particles.Length; i++ )
                {
                    particles[i].color = Color.Lerp( Color.white, Color.yellow, Random.Range( 0, 1.0f ) );
                }
                mParticleSystem.SetParticles( particles, particles.Length );
                */
            }
        }
    }
}
