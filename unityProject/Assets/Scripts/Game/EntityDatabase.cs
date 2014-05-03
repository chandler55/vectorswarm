﻿using UnityEngine;
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
        EntityType_MultiplierItem = 5,
        EntityType_ExplosionParticles = 6,
        EntityType_PlayerExplosion = 7,
    }

    public SimpleEnemy simpleEnemyPrefab;
    public FollowEnemy followEnemyPrefab;
    public ReverseEnemy reverseEnemyPrefab;
    public SineEnemy sineEnemyPrefab;
    public StationaryEnemy stationaryEnemyPrefab;

    public MultiplierItem multiplierItemPrefab;

    public ExplosionParticles explosionParticlesPrefab;
    public PlayerExplosion playerExplosionPrefab;


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
        ObjectPool.CreatePool( explosionParticlesPrefab );
        ObjectPool.CreatePool( playerExplosionPrefab );
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
            case EntityType.EntityType_ExplosionParticles:
                entity = explosionParticlesPrefab.Spawn( position, rotation ).gameObject;
                break;
            case EntityType.EntityType_PlayerExplosion:
                entity = playerExplosionPrefab.Spawn( position, rotation ).gameObject;
                break;
        }
        return entity;
    }
}