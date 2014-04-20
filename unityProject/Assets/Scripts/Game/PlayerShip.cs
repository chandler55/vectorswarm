using UnityEngine;
using System.Collections;

public class PlayerShip : Entity
{
    public GameObject bulletPrefab;

    private float mSpeed = 0.4f;

    // gun
    private const float mCooldownTime = 0.1f;
    private float mCoolDownTimeRemaining = 0;

    private static PlayerShip instance;
    public static PlayerShip Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }

    void Update()
    {
        Vector2 movementDirection = Vector2.zero;

        if ( Input.GetKey( KeyCode.A ) )
        {
            movementDirection.x -= 1;
        }
        if ( Input.GetKey( KeyCode.D ) )
        {
            movementDirection.x += 1;
        }
        if ( Input.GetKey( KeyCode.W ) )
        {
            movementDirection.y += 1;
        }
        if ( Input.GetKey( KeyCode.S ) )
        {
            movementDirection.y -= 1;
        }

        Velocity = mSpeed * movementDirection;
        Position += Velocity;

        if ( Velocity != Vector2.zero )
        {
            UpdateRotation();
        }

        // shoot bullet
        if ( mCoolDownTimeRemaining <= 0 )
        {
            mCoolDownTimeRemaining = mCooldownTime;
            ShootBullet();
        }

        if ( mCoolDownTimeRemaining > 0 )
        {
            mCoolDownTimeRemaining -= Time.deltaTime;
        }
    }

    void ShootBullet()
    {
        Vector2 target = (Vector2)Camera.main.ScreenToWorldPoint( Input.mousePosition ) - Position;
        target.Normalize();

        float randomSpread = Random.Range( -0.04f, 0.04f ) + Random.Range( -0.04f, 0.04f );
        Vector2 perpendicularOfTarget = new Vector2( target.y, -target.x );

        for ( int i = 0; i < 2; i++ )
        {
            GameObject bullet = Instantiate( bulletPrefab ) as GameObject;
            if ( bullet )
            {
                Bullet bulletComponent = bullet.GetComponent<Bullet>();
                if ( bulletComponent )
                {

                    bulletComponent.Position = Position + target * 1.0f;

                    // offset the two bullets
                    if ( i == 0 )
                    {
                        bulletComponent.Position += perpendicularOfTarget * 0.5f;
                    }
                    else
                    {
                        bulletComponent.Position -= perpendicularOfTarget * 0.5f;
                    }

                    bulletComponent.Velocity = target + perpendicularOfTarget * randomSpread;
                    bulletComponent.UpdateRotation();
                }
            }
        }
    }
}
