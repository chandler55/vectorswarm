using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{

    private float mShootDelay = 0.1f;
    private float mShootTimer = 0.0f;

    void Start()
    {

    }

    void Update()
    {
        // shooting logic
        mShootTimer += Time.deltaTime;
        if ( mShootTimer > mShootDelay )
        {
            mShootTimer = 0.0f;
            ShootGun();
        }
    }

    void ShootGun()
    {
        EntityDatabase.Instance.CreateEntity( EntityDatabase.EntityType.EntityType_Bullet, transform.position, Quaternion.identity );
    }
}
