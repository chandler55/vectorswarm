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
            //ShootGun();
        }
    }

    public void ShootGun( Vector2 direction )
    {
        GameObject bulletGo = EntityDatabase.Instance.CreateEntity( EntityDatabase.EntityType.EntityType_Bullet, transform.position, Quaternion.identity );
        if ( bulletGo )
        {
            bulletGo.transform.parent = bulletContainer;
            Bullet bullet = bulletGo.GetComponent<Bullet>();
            if ( bullet )
            {
                bullet.SetDirection( direction );
            }
        }
    }
}
