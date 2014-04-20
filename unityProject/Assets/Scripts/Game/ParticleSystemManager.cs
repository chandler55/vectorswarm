using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleSystemManager : MonoBehaviour
{
    private ParticleSystem mParticleSystem = null;
    private ParticleSystem.Particle[] mParticles;

    public enum ParticleType
    {
        None,
        Enemy,
        Bullet,
        IgnoreGravity,
    }

    public class GameParticle
    {
        public int index;
        public Vector2 Velocity;
        public Vector2 Position;

        public GameParticle()
        {
            index = -1;
            Velocity = Vector2.zero;
            Position = Vector2.zero;
        }

        public void UpdateParticle()
        {

        }
    }

    void Start()
    {
        mParticleSystem = GetComponentInChildren<ParticleSystem>();
        mParticles = new ParticleSystem.Particle[mParticleSystem.particleCount];

        if ( mParticleSystem )
        {
            mParticleSystem.GetParticles( mParticles );
        }
    }

    void Update()
    {
        if ( mParticleSystem )
        {
            mParticleSystem.GetParticles( mParticles );
            Debug.Log( mParticleSystem.particleCount );
        }
        /*
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[mParticleSystem.particleCount];
        mParticleSystem.GetParticles( particles );
        for ( int i = 0; i < particles.Length; i++ )
        {
            particles[i].color = Color.red;
        }
        mParticleSystem.SetParticles( particles, particles.Length );
        Debug.Log( particles.Length );
         * */
    }
}
