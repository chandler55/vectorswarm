using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleSystemManager : MonoBehaviour
{
    [HideInInspector]
    public const int MAX_PARTICLES = 2000;

    private static ParticleSystemManager instance;
    public static ParticleSystemManager Instance
    {
        get
        {
            return instance;
        }
    }

    private static ParticleSystem.Particle[] mParticles;
    private bool mParticleSystemInitialized = false;

    void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }

    public void CreateBulletExplosion(Vector3 pos)
    {
        GameObject go = EntityDatabase.Instance.CreateEntity( EntityDatabase.EntityType.EntityType_BulletExplosion, pos, Quaternion.identity );
        BulletExplosion explosionScript = go.GetComponent<BulletExplosion>();
    }

    public void CreateEnemyExplosion( Vector3 pos )
    {
        CreateEnemyExplosion( pos, false, Color.black );
    }

    public void CreateEnemyExplosion( Vector3 pos, bool overrideColor, Color color )
    {
        GameObject go = EntityDatabase.Instance.CreateEntity( EntityDatabase.EntityType.EntityType_ExplosionParticles, pos, Quaternion.identity );
        ExplosionParticles explosionScript = go.GetComponent<ExplosionParticles>();
        if ( explosionScript )
        {
            float hue1 = ( Mathf.Abs( Mathf.Sin( Time.realtimeSinceStartup ) ) * 6.0f + 3.0f ) % 6f;

            Color color1 = ColorUtil.HSVToColor( hue1, 0.5f, 1 );
            if ( overrideColor )
            {
                explosionScript.SetColor( color );
            }
            else
            {
                explosionScript.SetColor( color1 );
            }
        }
    }

    public void CreatePlayerExplosion( Vector3 pos )
    {
        EntityDatabase.Instance.CreateEntity( EntityDatabase.EntityType.EntityType_PlayerExplosion, pos, Quaternion.identity );
    }
}
