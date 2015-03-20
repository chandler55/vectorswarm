using UnityEngine;
using System.Collections;

public class Bullet : Entity
{
    public static Vector3 upVector = new Vector3( 0, 0, 0 );
    public float bulletSpeed = 60.0f;

    void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        UpdateRotation();
    }

    public void SetDirection( Vector2 direction )
    {
        Velocity = bulletSpeed * direction;
        UpdateRotation();
    }

    void Update()
    {
        // delete bullets that go off-screen
        if ( ( Position - PlayerSnake.Instance.Position ).magnitude > 120.0f )
        {
            //ParticleSystemManager.Instance.CreateBulletExplosion( Position );
            ObjectPool.Recycle( this );
            return;
        }

        Position += Velocity * Time.deltaTime;

        if ( !Playfield.Instance.WithinBoundary( Position ) )
        {
            Die();
        }

        //UpdateRotation();
    }

    void OnTriggerEnter2D( Collider2D collider )
    {
        if ( collider.tag == "Enemy" )
        {
            GameObject go = collider.gameObject;
            go.SendMessage( "DestroyEnemy" );
            ObjectPool.Recycle( this );
        }
    }

    void Die()
    {
        ParticleSystemManager.Instance.CreateBulletExplosion( gameObject.transform.position );
        ObjectPool.Recycle( this );
    }
}
