using UnityEngine;
using System.Collections;

public class Bullet : Entity
{
    public static Vector3 upVector = new Vector3( 0, 0, 0 );
    public Vector3 initialVelocity = Vector3.zero;

    void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        Velocity = initialVelocity;
        UpdateRotation();
    }

    void Update()
    {
        
        // delete bullets that go off-screen
        if ( (Position - PlayerSnake.Instance.Position).magnitude > 30.0f )
        {
            //ParticleSystemManager.Instance.CreateBulletExplosion( Position );
            ObjectPool.Recycle( this );
            return;
        }
        

        Position += Velocity;

        UpdateRotation();
    }

    public override void CollisionTriggered( Collider2D collider )
    {
        if ( collider.tag == "Enemy" )
        {
            GameObject go = collider.gameObject;
            go.SendMessage( "DestroyEnemy" );
            ObjectPool.Recycle( this );
        }
    }
}
