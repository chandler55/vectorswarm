using UnityEngine;
using System.Collections;

public class InstantiatePrefab : MonoBehaviour
{
    public GameObject prefab;

    void Start()
    {
        GameObject go = Instantiate( prefab, transform.position, Quaternion.identity ) as GameObject;
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
