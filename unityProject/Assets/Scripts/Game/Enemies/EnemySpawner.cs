using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    private const float MIN_DISTANCE_FROM_PLAYER_FOR_SPAWNING = 100.0f;

    private float mInverseSpawnChance = 60;

    void Start()
    {

    }

    void Update()
    {
        if ( mInverseSpawnChance > 20 )
        {
            mInverseSpawnChance -= 0.3f * Time.deltaTime;
        }

        if ( Random.Range( 0, (int)mInverseSpawnChance ) == 0 )
        {
            CreateEnemy( GetSpawnPosition() );
        }
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

    void CreateEnemy( Vector2 pos )
    {
        GameObject enemy = Instantiate( enemyPrefab, pos, Quaternion.identity ) as GameObject;
    }

    void Reset()
    {
        mInverseSpawnChance = 60;
    }
}
