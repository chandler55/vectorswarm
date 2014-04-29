using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Entity
{
    private float mStartingDuration = 1.0f;
    private float mTimeToStart = 0.0f;

    private tk2dSprite mSprite = null;
    private List<IEnumerator<int>> mBehaviours = new List<IEnumerator<int>>();

    void Start()
    {
        mSprite = GetComponentInChildren<tk2dSprite>();
        mSprite.color = Color.clear;

        mTimeToStart = mStartingDuration;

        AddBehaviour( FollowPlayer() );
    }

    void Update()
    {
        if ( mTimeToStart > 0 )
        {
            mTimeToStart -= Time.deltaTime;

            if ( mSprite )
            {
                mSprite.color = Color.white * ( mStartingDuration - mTimeToStart );
            }
        }
        else
        {
            ApplyBehaviours();
        }

        Position += Velocity * Time.deltaTime;
        Velocity *= 0.8f;

        UpdateRotation();
    }

    private void AddBehaviour( IEnumerable<int> behaviour )
    {
        mBehaviours.Add( behaviour.GetEnumerator() );
    }

    private void ApplyBehaviours()
    {
        for ( int i = 0; i < mBehaviours.Count; i++ )
        {
            if ( !mBehaviours[i].MoveNext() )
            {
                mBehaviours.RemoveAt( i-- );
            }
        }
    }

    IEnumerable<int> FollowPlayer( float acceleration = 0.1f )
    {
        while ( true )
        {
            Velocity += ( PlayerShip.Instance.Position - Position ) * acceleration * Time.deltaTime;
            yield return 0;
        }
    }

    IEnumerable<int> MoveInASquare()
    {
        const int framesPerSide = 30;
        while ( true )
        {
            // move right for 30 frames
            for ( int i = 0; i < framesPerSide; i++ )
            {
                Velocity = new Vector2( 1, 0 );
                yield return 0;
            }

            // move down
            for ( int i = 0; i < framesPerSide; i++ )
            {
                Velocity = new Vector2( 0, 1 );
                yield return 0;
            }

            // move left
            for ( int i = 0; i < framesPerSide; i++ )
            {
                Velocity = new Vector2( -1, 0 );
                yield return 0;
            }

            // move up
            for ( int i = 0; i < framesPerSide; i++ )
            {
                Velocity = new Vector2( 0, -1 );
                yield return 0;
            }
        }
    }

    public override void CollisionTriggered( Collider2D collider )
    {
        switch ( collider.tag )
        {
            case "Bullet":
                ParticleSystemManager.Instance.CreateEnemyExplosion( Position );
                Destroy( gameObject );
                break;
            case "Enemy":
                Vector3 d = gameObject.transform.position - collider.gameObject.transform.position;
                Velocity += (Vector2)( 0.5f * d / ( d.sqrMagnitude + 1 ) );
                break;
        }
    }

    /*
    IEnumerable<int> MoveRandomly()
    {
        float direction = rand.NextFloat( 0, MathHelper.TwoPi );

        while ( true )
        {
            direction += rand.NextFloat( -0.1f, 0.1f );
            direction = MathHelper.WrapAngle( direction );

            for ( int i = 0; i < 6; i++ )
            {
                Velocity += MathUtil.FromPolar( direction, 0.4f );
                Orientation -= 0.05f;

                var bounds = GameRoot.Viewport.Bounds;
                bounds.Inflate( -image.Width, -image.Height );

                // if the enemy is outside the bounds, make it move away from the edge
                if ( !bounds.Contains( Position.ToPoint() ) )
                    direction = ( GameRoot.ScreenSize / 2 - Position ).ToAngle() + rand.NextFloat( -MathHelper.PiOver2, MathHelper.PiOver2 );

                yield return 0;
            }
        }
    }*/
}
