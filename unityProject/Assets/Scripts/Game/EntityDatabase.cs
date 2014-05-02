using UnityEngine;
using System.Collections;

public class EntityDatabase : MonoBehaviour
{
    private static EntityDatabase instance;
    public static EntityDatabase Instance
    {
        get
        {
            return instance;
        }
    }

    public enum EntityType
    {
        EntityType_SimpleEnemy,
        EntityType_FollowEnemy,
        EntityType_ReverseEnemy,
        EntityType_SineEnemy,
        EntityType_StationaryEnemy,
        EntityType_MultiplierItem,
    }

    public SimpleEnemy simpleEnemyPrefab;
    public FollowEnemy followEnemyPrefab;
    public ReverseEnemy reverseEnemyPrefab;
    public SineEnemy sineEnemyPrefab;
    public StationaryEnemy stationaryEnemyPrefab;
    public MultiplierItem multiplierItemPrefab;

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
        ObjectPool.CreatePool( simpleEnemyPrefab );
        ObjectPool.CreatePool( followEnemyPrefab );
        ObjectPool.CreatePool( reverseEnemyPrefab );
        ObjectPool.CreatePool( sineEnemyPrefab );
        ObjectPool.CreatePool( stationaryEnemyPrefab );
        ObjectPool.CreatePool( multiplierItemPrefab );
    }

    public GameObject CreateEntity(EntityType entityType)
    {
        return CreateEntity( entityType, Vector3.zero, Quaternion.identity );
    }

    public GameObject CreateEntity( EntityType entityType, Vector3 position, Quaternion rotation )
    {
        GameObject entity = null;
        switch (entityType)
        {
            case EntityType.EntityType_SimpleEnemy:
                entity = simpleEnemyPrefab.Spawn(position, rotation).gameObject;
                break;
            case EntityType.EntityType_FollowEnemy:
                entity = followEnemyPrefab.Spawn( position, rotation ).gameObject;
                break;
            case EntityType.EntityType_ReverseEnemy:
                entity = reverseEnemyPrefab.Spawn( position, rotation ).gameObject;
                break;
            case EntityType.EntityType_SineEnemy:
                entity = sineEnemyPrefab.Spawn( position, rotation ).gameObject;
                break;
            case EntityType.EntityType_StationaryEnemy:
                entity = stationaryEnemyPrefab.Spawn( position, rotation ).gameObject;
                break;
            case EntityType.EntityType_MultiplierItem:
                entity = multiplierItemPrefab.Spawn( position, rotation ).gameObject;
                break;
        }
        return entity;
    }
}
