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

        // exhaust
        MakeExhaustFire();
    }

    private void MakeExhaustFire()
    {
        if ( Velocity.sqrMagnitude > 0.1f )
        {
            // set up some variables
            Matrix4x4
            float t = Time.realtimeSinceStartup;
            // The primary velocity of the particles is 3 pixels/frame in the direction opposite to which the ship is travelling.
            Vector2 baseVel = Velocity * -3.0f;

            // Calculate the sideways velocity for the two side streams. The direction is perpendicular to the ship's velocity and the
            // magnitude varies sinusoidally.
            Vector2 perpVel = new Vector2( baseVel.y, -baseVel.x ) * ( 0.6f * (float)Mathf.Sin( t * 10.0f ) );

            Color sideColor = new Color( 200 / 255.0f, 38 / 255.0f, 9 / 255.0f );    // deep red
            Color midColor = new Color( 255 / 255.0f, 187 / 255.0f, 30 / 255.0f );   // orange-yellow
            Vector2 pos = Position;   // position of the ship's exhaust pipe.
            const float alpha = 0.7f;

            // middle particle stream
            Vector2 velMid = baseVel + GameUtils.RandomVector2( 0, 1 );

            ParticleSystemManager.Instance.CreateParticle( pos, perpVel, Color.white * alpha, 1.0f, new Vector2( 0.5f, 1 ), 0 );
            //ParticleSystemManager.Instance.CreateParticle( pos, velMid, midColor * alpha, 1.0f, new Vector2( 0.5f, 1 ), 0 );

            // side particle streams
            Vector2 vel1 = baseVel + perpVel + GameUtils.RandomVector2( 0, 0.3f );
            Vector2 vel2 = baseVel - perpVel + GameUtils.RandomVector2( 0, 0.3f );

            //ParticleSystemManager.Instance.CreateParticle( pos, vel1, Color.white * alpha, 1.0f, new Vector2( 0.5f, 1 ), 0 );
            //ParticleSystemManager.Instance.CreateParticle( pos, vel2, Color.white * alpha, 1.0f, new Vector2( 0.5f, 1 ), 0 );

            //ParticleSystemManager.Instance.CreateParticle( pos, vel1, sideColor * alpha, 1.0f, new Vector2( 0.5f, 1 ), 0 );
            //ParticleSystemManager.Instance.CreateParticle( pos, vel2, sideColor * alpha, 1.0f, new Vector2( 0.5f, 1 ), 0 );
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

    void CollisionTriggered( Collider2D collider )
    {
        ParticleSystemManager.Instance.CreatePlayerExplision( Position );
    }
}
