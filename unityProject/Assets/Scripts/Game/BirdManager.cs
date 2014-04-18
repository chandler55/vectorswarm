using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdManager : MonoBehaviour
{
    private static float DISTANCE_BETWEEN_BIRDS = 3.0f;

    public GameObject birdPrefab;

    private List<Bird> birdsList = new List<Bird>();
    
    void Start()
    {
        for ( int i = 0; i < 100; i++ )
        {
            GameObject bird = Instantiate( birdPrefab, Vector2.zero, Quaternion.identity ) as GameObject;

            // set parent
            bird.transform.parent = gameObject.transform;

            Bird birdComponent = bird.GetComponent<Bird>();
            if ( birdComponent )
            {
                birdComponent.Position = new Vector2( UnityEngine.Random.Range( -10.0f, 10.0f ), UnityEngine.Random.Range( -10.0f, 10.0f ) );
                birdsList.Add( birdComponent );
            }
        }
    }

    void Update()
    {
        //if ( Input.GetKeyUp( KeyCode.A ) )
        {
            foreach ( Bird bird in birdsList )
            {
                Vector2 flyTowardCentre = FlyTowardCentreOfMass( bird );
                Vector2 keepAway = KeepAwayFromOtherBirds( bird );
                Vector2 matchVelocity = MatchVelocityWithOtherBirds( bird );
                Vector2 tendTowardsPlayer = Seek( Camera.main.ScreenToWorldPoint( Input.mousePosition ), bird );

                bird.Velocity += flyTowardCentre + keepAway + tendTowardsPlayer + matchVelocity;

                bird.Position += bird.Velocity * Time.deltaTime;
            }
        }
    }

    Vector2 FlyTowardCentreOfMass( Bird bird )
    {
        Vector2 centreOfMass = Vector2.zero;

        foreach ( Bird b in birdsList )
        {
            if ( b != bird )
            {
                centreOfMass += b.Position;
            }
        }

        if ( birdsList.Count >= 2 )
        {
            centreOfMass /= birdsList.Count - 1;
        }

        return Seek( centreOfMass - bird.Position, bird );
    }

    Vector2 KeepAwayFromOtherBirds( Bird bird )
    {
        Vector2 keepAwayVector = Vector2.zero;
        foreach ( Bird b in birdsList )
        {
            if ( bird != b )
            {
                if ( ( b.Position - bird.Position ).magnitude < DISTANCE_BETWEEN_BIRDS )
                {
                    keepAwayVector -= ( b.Position - bird.Position );
                }
            }
        }

        return keepAwayVector;
    }

    Vector2 MatchVelocityWithOtherBirds( Bird bird )
    {
        Vector2 matchVelocity = Vector2.zero;
        foreach ( Bird b in birdsList )
        {
            if ( bird != b )
            {
                matchVelocity += b.Velocity;
            }
        }

        if ( birdsList.Count >= 2 )
        {
            matchVelocity /= birdsList.Count - 1;
        }

        return ( matchVelocity - bird.Velocity ) / 8.0f;
    }

    Vector2 Seek( Vector2 target, Bird b )
    {
        Vector2 steer = Vector2.zero;
        Vector2 desired = target - b.Position;

        desired.Normalize();
        desired *= b.MaxSpeed;

        steer = desired - b.Velocity;
        if ( steer.magnitude > b.MaxSteerForce )
        {
            steer.Normalize();
            steer *= b.MaxSteerForce;
        }

        return steer;
    }
}
