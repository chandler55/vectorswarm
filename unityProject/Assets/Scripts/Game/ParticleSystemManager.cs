using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleSystemManager : MonoBehaviour
{
    private ParticleSystem mParticleSystem = null;

    void Start()
    {
        mParticleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[mParticleSystem.particleCount];
        mParticleSystem.GetParticles( particles );
        for ( int i = 0; i < particles.Length; i++ )
        {
            particles[i].color = Color.red;
        }
        mParticleSystem.SetParticles( particles, particles.Length );
        Debug.Log( particles.Length );
    }
}
