using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public TMPro.TextMeshPro debugText;

    private float mInverseSpawnChance = 60;

    void Start()
    {
        Messenger.AddListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
    }

    void Update()
    {
        if ( mInverseSpawnChance > 20 )
        {
            mInverseSpawnChance -= 0.3f * Time.deltaTime;
        }

        if ( Random.Range( 0, (int)mInverseSpawnChance ) == 0 )
        {
            SpawnEnemy();
        }
    }

    void OnNewGameStarted()
    {
        mInverseSpawnChance = 60.0f;
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        Vector3 randomPos = Playfield.Instance.GetRandomPos();
        GameObject go = EntityDatabase.Instance.CreateEntity( EntityDatabase.EntityType.EntityType_SlowerSimpleEnemy, randomPos, Quaternion.identity );
        go.transform.parent = gameObject.transform;

        debugText.text = GetComponentsInChildren<Entity>().Length.ToString();
    }
}
