using UnityEngine;
using System.Collections;

public class Bullet : Entity
{
    public static Vector3 upVector = new Vector3( 0, 0, 0 );

    void Start()
    {

    }

    void Update()
    {
        // delete bullets that go off-screen
        if ( !GameSettings.WORLD_BOUNDARY.Contains( Position ) )
        {
            ParticleSystemManager.Instance.CreateBulletExplosion( Position );
            Destroy( gameObject );
            return;
        }

        Position += Velocity;

        UpdateRotation();
    }

}
