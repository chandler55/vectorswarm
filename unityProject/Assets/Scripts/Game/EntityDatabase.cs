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
        EntityType_SimpleEnemy = 0,
        EntityType_FollowEnemy = 1,
        EntityType_ReverseEnemy = 2,
        EntityType_SineEnemy = 3,
        EntityType_StationaryEnemy = 4,
        EntityType_LeverEnemy = 5,
        EntityType_ShortPathEnemy = 6,
        EntityType_VerticalEnemy = 7,
        EntityType_SlowerSimpleEnemy = 8,
        EntityType_MovingStationaryEnemy = 9,
        EntityType_HourGlassEnemy = 10,
        EntityType_TriangleSineEnemy = 11,

        EntityType_MultiplierItem = 100,
        EntityType_ScreenBomb = 101,
        EntityType_Bullet = 102,

        EntityType_ExplosionParticles = 200,
        EntityType_PlayerExplosion = 201,

        EntityType_ScoreIndicator = 300,
    }

    public SimpleEnemy              simpleEnemyPrefab;
    public FollowEnemy              followEnemyPrefab;
    public ReverseEnemy             reverseEnemyPrefab;
    public SineEnemy                sineEnemyPrefab;
    public StationaryEnemy          stationaryEnemyPrefab;
    public LeverEnemy               leverEnemyPrefab;
    public ShortPathEnemy           shortPathEnemyPrefab;
    public VerticalEnemy            verticalEnemyPrefab;
    public SlowerSimpleEnemy        slowerSimpleEnemyPrefab;
    public MovingStationaryEnemy    movingStationaryEnemyPrefab;
    public HourGlassEnemy           hourGlassEnemyPrefab;
    public TriangleSineEnemy        triangleSineEnemyPrefab;

    public MultiplierItem   multiplierItemPrefab;
    public ScreenBomb       screenBombPrefab;
    public Bullet           bulletPrefab;

    public ExplosionParticles   explosionParticlesPrefab;
    public PlayerExplosion      playerExplosionPrefab;

    public ScoreIndicator      scoreIndicatorPrefab;


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
        ObjectPool.CreatePool( simpleEnemyPrefab, 20 );
        ObjectPool.CreatePool( followEnemyPrefab );
        ObjectPool.CreatePool( reverseEnemyPrefab, 20 );
        ObjectPool.CreatePool( sineEnemyPrefab, 20 );
        ObjectPool.CreatePool( stationaryEnemyPrefab, 20 );
        ObjectPool.CreatePool( leverEnemyPrefab, 20 );
        ObjectPool.CreatePool( shortPathEnemyPrefab, 20 );
        ObjectPool.CreatePool( verticalEnemyPrefab, 20 );
        ObjectPool.CreatePool( slowerSimpleEnemyPrefab );
        ObjectPool.CreatePool( movingStationaryEnemyPrefab, 20 );
        ObjectPool.CreatePool( hourGlassEnemyPrefab, 20 );
        ObjectPool.CreatePool( triangleSineEnemyPrefab, 20 );

        ObjectPool.CreatePool( multiplierItemPrefab, 20 );
        ObjectPool.CreatePool( screenBombPrefab, 1 );
        ObjectPool.CreatePool( bulletPrefab, 10 );

        ObjectPool.CreatePool( explosionParticlesPrefab, 5 );
        ObjectPool.CreatePool( playerExplosionPrefab, 1 );

        ObjectPool.CreatePool( scoreIndicatorPrefab, 5 );
    }

    public GameObject CreateEntity( EntityType entityType )
    {
        return CreateEntity( entityType, Vector3.zero, Quaternion.identity );
    }

    public GameObject CreateEntity( EntityType entityType, Vector3 position, Quaternion rotation )
    {
        GameObject entity = null;
        switch ( entityType )
        {
            case EntityType.EntityType_SimpleEnemy:
                entity = simpleEnemyPrefab.Spawn( position, rotation ).gameObject;
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
            case EntityType.EntityType_LeverEnemy:
                entity = leverEnemyPrefab.Spawn( position, rotation ).gameObject;
                break;
            case EntityType.EntityType_ShortPathEnemy:
                entity = shortPathEnemyPrefab.Spawn( position, rotation ).gameObject;
                break;
            case EntityType.EntityType_VerticalEnemy:
                entity = verticalEnemyPrefab.Spawn( position, rotation ).gameObject;
                break;
            case EntityType.EntityType_SlowerSimpleEnemy:
                entity = slowerSimpleEnemyPrefab.Spawn( position, rotation ).gameObject;
                break;
            case EntityType.EntityType_MovingStationaryEnemy:
                entity = movingStationaryEnemyPrefab.Spawn( position, rotation ).gameObject;
                break;
            case EntityType.EntityType_HourGlassEnemy:
                entity = hourGlassEnemyPrefab.Spawn( position, rotation ).gameObject;
                break;
            case EntityType.EntityType_TriangleSineEnemy:
                entity = triangleSineEnemyPrefab.Spawn( position, rotation ).gameObject;
                break;

            case EntityType.EntityType_MultiplierItem:
                entity = multiplierItemPrefab.Spawn( position, rotation ).gameObject;
                break;
            case EntityType.EntityType_ScreenBomb:
                entity = screenBombPrefab.Spawn( position, rotation ).gameObject;
                break;
            case EntityType.EntityType_Bullet:
                entity = bulletPrefab.Spawn( position, rotation ).gameObject;
                break;

            case EntityType.EntityType_ExplosionParticles:
                entity = explosionParticlesPrefab.Spawn( position, rotation ).gameObject;
                break;
            case EntityType.EntityType_PlayerExplosion:
                entity = playerExplosionPrefab.Spawn( position, rotation ).gameObject;
                break;
            case EntityType.EntityType_ScoreIndicator:
                entity = scoreIndicatorPrefab.Spawn( position, rotation ).gameObject;
                break;
        }

        return entity;
    }
}
