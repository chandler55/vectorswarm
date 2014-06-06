using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public Transform bulletContainer;

    private float mShootDelay = 0.1f;
    private float mShootTimer = 0.0f;

    void Start()
    {
        GameUtils.Assert( bulletContainer );
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
        GameObject bulletGo = EntityDatabase.Instance.CreateEntity( EntityDatabase.EntityType.EntityType_Bullet, transform.position, Quaternion.identity );
        if ( bulletGo )
        {
            bulletGo.transform.parent = bulletContainer;
        }
    }
}
