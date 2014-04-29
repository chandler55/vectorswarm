using UnityEngine;
using System.Collections;

public class SineEnemy : Entity
{
    public float waveHeight = 6.0f;

    private bool mMovingRight = false;
    private const float SPEED = 30.0f;
    private float mBaselineY = 0.0f;

    void Start()
    {
        mMovingRight = Position.x > 0;
        mBaselineY = Position.y;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if ( mMovingRight )
        {
            if ( Position.x >= GameSettings.WORLD_BOUNDARY.x + GameSettings.WORLD_BOUNDARY.width )
            {
                mMovingRight = false;
            }
        }
        else
        {
            if ( Position.x <= GameSettings.WORLD_BOUNDARY.x )
            {
                mMovingRight = true;
            }
        }

        Vector3 newPos;
        if ( mMovingRight )
        {
            newPos = new Vector3( GameSettings.WORLD_BOUNDARY.x + GameSettings.WORLD_BOUNDARY.width, Position.y, gameObject.transform.position.z );
        }
        else
        {
            newPos = new Vector3( GameSettings.WORLD_BOUNDARY.x, Position.y, gameObject.transform.position.z );
        }

        Velocity = (Vector2)newPos - Position;

        Velocity = Velocity.normalized * SPEED;

        float timeSinceStartup = Time.realtimeSinceStartup;
        Velocity = new Vector2( Velocity.x, ( ( mBaselineY - Position.y ) + Mathf.Sin( timeSinceStartup * 10.0f ) * waveHeight ) );

        Position += Velocity * Time.deltaTime;

        //Go.to( transform, movementDuration, new GoTweenConfig().position( newPos ) ).setOnCompleteHandler( OnCompleteTween );
    }

    public override void CollisionTriggered( Collider2D collider )
    {
        ParticleSystemManager.Instance.CreateEnemyExplosion( Position );
        Destroy( gameObject );
    }
}
