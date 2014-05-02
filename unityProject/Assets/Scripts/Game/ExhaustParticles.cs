using UnityEngine;
using System.Collections;

public class ExhaustParticles : MonoBehaviour
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
            Color exhaustColor = ColorUtil.HSVToColor( Mathf.Abs( Mathf.Sin( Time.realtimeSinceStartup ) ) * 6.0f, 0.5f, 1.0f );
            mParticleSystem.startColor = exhaustColor;
        }

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
