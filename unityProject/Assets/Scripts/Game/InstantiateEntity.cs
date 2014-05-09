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
        }
    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube( transform.position, Vector3.one );

    }
}
