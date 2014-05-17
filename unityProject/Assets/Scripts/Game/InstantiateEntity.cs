using UnityEngine;
using System.Collections;

public class InstantiateEntity : MonoBehaviour
{
    public EntityDatabase.EntityType entityType;

    public bool simpleMoveRight = false;

    void Start()
    {
        GameObject go = EntityDatabase.Instance.CreateEntity( entityType, transform.position, Quaternion.identity );
        go.transform.parent = transform;

        switch ( entityType )
        {
            case EntityDatabase.EntityType.EntityType_SimpleEnemy:
                SimpleEnemy simpleEnemy = go.GetComponent<SimpleEnemy>();
                if ( simpleEnemy )
                {
                    simpleEnemy.SetMovingRight( simpleMoveRight );
                }
                break;
            case EntityDatabase.EntityType.EntityType_SlowerSimpleEnemy:
                SlowerSimpleEnemy slowerSimpleEnemy = go.GetComponent<SlowerSimpleEnemy>();
                if ( slowerSimpleEnemy )
                {
                    slowerSimpleEnemy.SetMovingRight( simpleMoveRight );
                }
                break;
        }
    }

    void Update()
    {

    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube( transform.position, Vector3.one );

        switch ( entityType )
        {
            case EntityDatabase.EntityType.EntityType_SimpleEnemy:
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere( transform.position, 1 );
                break;
            case EntityDatabase.EntityType.EntityType_FollowEnemy:
                break;
            case EntityDatabase.EntityType.EntityType_ReverseEnemy:
                Gizmos.color = new Color( 255, 168, 0 );
                Gizmos.DrawWireSphere( transform.position, 0.5f );
                break;
            case EntityDatabase.EntityType.EntityType_SineEnemy:
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere( transform.position, 1.0f );
                break;
            case EntityDatabase.EntityType.EntityType_StationaryEnemy:
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere( transform.position, 0.5f );
                break;
            case EntityDatabase.EntityType.EntityType_LeverEnemy:
                Gizmos.color = new Color( 255, 0, 255 );
                Gizmos.DrawWireSphere( transform.position, 4.8f );
                break;
            case EntityDatabase.EntityType.EntityType_ShortPathEnemy:
                Gizmos.color = new Color( 255, 0, 255 );
                Gizmos.DrawWireSphere( transform.position, 1.5f );
                break;
            case EntityDatabase.EntityType.EntityType_VerticalEnemy:
                Gizmos.color = new Color( 255, 155, 0 );
                Gizmos.DrawWireSphere( transform.position, 1.5f );
                break;
            case EntityDatabase.EntityType.EntityType_SlowerSimpleEnemy:
                Gizmos.color = new Color( 127, 0, 255 );
                Gizmos.DrawWireSphere( transform.position, 1.6f );
                break;
            case EntityDatabase.EntityType.EntityType_MovingStationaryEnemy:
                Gizmos.color = new Color( 255, 0, 255 );
                Gizmos.DrawWireSphere( transform.position, 0.5f );
                break;
            case EntityDatabase.EntityType.EntityType_HourGlassEnemy:
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere( transform.position, 1.5f );
                break;
            case EntityDatabase.EntityType.EntityType_TriangleSineEnemy:
                Gizmos.color = new Color( 255, 255, 0 );
                Gizmos.DrawWireSphere( transform.position, 1.0f );
                break;
            case EntityDatabase.EntityType.EntityType_MultiplierItem:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere( transform.position, 0.5f );
                break;
        }
    }
#endif

}
