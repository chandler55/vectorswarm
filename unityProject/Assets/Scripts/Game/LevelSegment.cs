using UnityEngine;
using System.Collections;

public class LevelSegment : MonoBehaviour
{
    public bool shouldIncludeAfterburnerPowerup = false;

    void Start()
    {
        if ( shouldIncludeAfterburnerPowerup )
        {
            // change one of the multiplier items into afterburner powerups
            InstantiateEntity[] instantiateEntities = GetComponentsInChildren<InstantiateEntity>();
            if ( instantiateEntities.Length > 0 )
            {
                int randomChoice = Random.Range( 0, instantiateEntities.Length );
                instantiateEntities[randomChoice].entityType = EntityDatabase.EntityType.EntityType_AfterburnerItem;
            }
        }
    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Vector3 bottomBoundary = transform.position + new Vector3( 0, 0, 0 );
        Vector3 topBoundary = transform.position + new Vector3( 0, GameSettings.LEVEL_SEGMENT_SIZE_Y, 0 );
        Gizmos.DrawLine( topBoundary, bottomBoundary );

        Gizmos.DrawLine( topBoundary + new Vector3( -100, 0, 0 ), topBoundary + new Vector3( 100, 0, 0 ) );
        Gizmos.DrawLine( bottomBoundary + new Vector3( -100, 0, 0 ), bottomBoundary + new Vector3( 100, 0, 0 ) );

        Gizmos.DrawLine( topBoundary + new Vector3( GameSettings.WORLD_BOUNDARY.x, 0, 0 ), bottomBoundary + new Vector3( GameSettings.WORLD_BOUNDARY.x, 0, 0 ) );

        Gizmos.DrawLine( topBoundary + new Vector3( GameSettings.WORLD_BOUNDARY.x + GameSettings.WORLD_BOUNDARY.width, 0, 0 ),
            bottomBoundary + new Vector3( GameSettings.WORLD_BOUNDARY.x + GameSettings.WORLD_BOUNDARY.width, 0, 0 ) );

    }
}
