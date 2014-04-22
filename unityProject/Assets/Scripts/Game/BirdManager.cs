using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdManager : MonoBehaviour
{
    public GUIText birdCountText;

    private static BirdManager instance;
    public static BirdManager Instance
    {
        get
        {
            return instance;
        }
    }

    public float MIN_DISTANCE_BETWEEN_BIRDS = 2.0f;
    private const float MIN_DISTANCE_FROM_PLAYER_FOR_SPAWNING = 100.0f;

    public GameObject birdPrefab;

    private List<Bird> mBirdsList = new List<Bird>();
    private float mInverseSpawnChance = 60;

    void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }

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
                mBirdsList.Add( birdComponent );
            }
        }
    }

    void Update()
    {
        if ( mInverseSpawnChance > 20 )
        {
            mInverseSpawnChance -= 0.3f * Time.deltaTime;
        }

        if ( Random.Range( 0, (int)mInverseSpawnChance ) == 0 )
        {
            CreateBird( GetSpawnPosition() );
        }

        // update the birds!
        foreach ( Bird bird in mBirdsList )
        {
            //Vector2 flyTowardCentre = FlyTowardCentreOfMass( bird );
            //Vector2 keepAway = KeepAwayFromOtherBirds( bird ) * 5.0f;
            //Vector2 matchVelocity = MatchVelocityWithOtherBirds( bird );
            Vector2 tendTowardsPlayer = Seek( PlayerShip.Instance.Position, bird );

            //Vector2 randomJitter = new Vector2( Random.Range( -0.1f, 0.1f ), Random.Range( -0.1f, 0.1f ) );

            bird.Velocity += tendTowardsPlayer;// +flyTowardCentre + keepAway + matchVelocity;

            bird.Position += bird.Velocity * Time.deltaTime;
        }

        birdCountText.text = mBirdsList.Count.ToString();
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 pos;

        while ( true )
        {
            pos = new Vector2( Random.Range( GameSettings.WORLD_BOUNDARY.x, GameSettings.WORLD_BOUNDARY.width ),
                                Random.Range( GameSettings.WORLD_BOUNDARY.y, GameSettings.WORLD_BOUNDARY.height ) );

            float distance = ( (Vector2)( pos - PlayerShip.Instance.Position ) ).sqrMagnitude;

            if ( distance > MIN_DISTANCE_FROM_PLAYER_FOR_SPAWNING )
            {
                break;
            }
        }

        return pos;
    }

    void CreateBird( Vector2 pos )
    {
        GameObject bird = Instantiate( birdPrefab, pos, Quaternion.identity ) as GameObject;

        // set parent
        bird.transform.parent = gameObject.transform;

        Bird birdComponent = bird.GetComponent<Bird>();
        if ( birdComponent )
        {
            mBirdsList.Add( birdComponent );
        }
    }

    Vector2 FlyTowardCentreOfMass( Bird bird )
    {
        Vector2 centreOfMass = Vector2.zero;

        foreach ( Bird b in mBirdsList )
        {
            if ( b != bird )
            {
                centreOfMass += b.Position;
            }
        }

        if ( mBirdsList.Count >= 2 )
        {
            centreOfMass /= mBirdsList.Count - 1;
        }

        return Seek( centreOfMass - bird.Position, bird );
    }

    Vector2 KeepAwayFromOtherBirds( Bird bird )
    {
        Vector2 keepAwayVector = Vector2.zero;

        int count = 0;
        foreach ( Bird b in mBirdsList )
        {
            if ( bird != b )
            {
                float distance = ( b.Position - bird.Position ).magnitude;
                if ( distance < MIN_DISTANCE_BETWEEN_BIRDS )
                {
                    Vector2 diffVector = ( bird.Position - b.Position );

                    diffVector.Normalize();
                    //diffVector /= distance;
                    keepAwayVector += diffVector;
                    count++;
                }
            }
        }
        

        if ( count > 0 )
        {
            keepAwayVector /= count;
        }

        // As long as the vector is greater than 0
        if ( keepAwayVector.magnitude > 0 )
        {
            // First two lines of code below could be condensed with new PVector setMag() method
            // Not using this method until Processing.js catches up
            // steer.setMag(maxspeed);

            // Implement Reynolds: Steering = Desired - Velocity
            keepAwayVector.Normalize();
            keepAwayVector *= bird.MaxSpeed;
            keepAwayVector -= bird.Velocity;
            if ( keepAwayVector.magnitude > bird.MaxSteerForce )
            {
                keepAwayVector.Normalize();
                keepAwayVector *= bird.MaxSteerForce;
            } 
        }

        return keepAwayVector;
    }

    Vector2 MatchVelocityWithOtherBirds( Bird bird )
    {
        Vector2 matchVelocity = Vector2.zero;
        foreach ( Bird b in mBirdsList )
        {
            if ( bird != b )
            {
                matchVelocity += b.Velocity;
            }
        }

        if ( mBirdsList.Count >= 2 )
        {
            matchVelocity /= mBirdsList.Count - 1;
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

    public List<Bird> GetBirdsList()
    {
        return mBirdsList;
    }

    public void DestroyBird( Bird b )
    {
        mBirdsList.Remove( b );
        Destroy( b.gameObject );
    }

}
