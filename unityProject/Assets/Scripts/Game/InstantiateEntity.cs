using UnityEngine;
using System.Collections;

public class InstantiateEntity : MonoBehaviour
{
    public EntityDatabase.EntityType entityType;

    void Start()
    {
        GameObject go = EntityDatabase.Instance.CreateEntity( entityType, transform.position, Quaternion.identity );
        go.transform.parent = transform;
    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube( transform.position, Vector3.one );
    }
}
