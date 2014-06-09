using UnityEngine;
using System.Collections;

public class InstantiateEntity : MonoBehaviour
{
    public EntityDatabase.EntityType entityType;

    public bool simpleMoveRight = false;

    private bool mEntityCreated = false;
    private bool mRecycled = false;

    void Start()
    {
    }

    void Update()
    {
        float distance = transform.position.y - PlayerSnake.Instance.Position.y;

        if ( !mEntityCreated )
        {
            if ( distance < 50 )
            {
                mEntityCreated = true;
                CreateEntity();
            }
        }
        else if ( mEntityCreated && !mRecycled )
        {
            if ( distance < -50 )
            {
                mRecycled = true;

                Entity[] entities = gameObject.GetComponentsInChildren<Entity>();
                foreach ( Entity entity in entities )
                {
                    entity.SendMessage( "Die" );
                }
            }
        }
    }

    void CreateEntity()
    {
        Vector3 rotation = Vector3.zero;
        if ( entityType == EntityDatabase.EntityType.EntityType_MultiplierItem )
        {
            rotation = new Vector3( 0, 0, Random.Range( 0, 360.0f ) );
        }
        GameObject go = EntityDatabase.Instance.CreateEntity( entityType, transform.position, Quaternion.Euler( rotation ) );
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
