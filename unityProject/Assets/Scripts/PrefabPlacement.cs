using UnityEngine;
using System.Collections;

public class PrefabPlacement : MonoBehaviour
{

    public GameObject somePrefab;
    public GameObject instance { get; private set; }


    void Awake()
    {

#if UNITY_EDITOR
        // In case some previous instance didn't get destroyed...
        while ( transform.childCount > 0 )
        {
            DestroyImmediate( transform.GetChild( 0 ).gameObject );
        }
#endif

        instance = Instantiate( somePrefab, transform.position, transform.rotation ) as GameObject;
        instance.transform.parent = transform;
        instance.transform.localScale = Vector3.one;
    }



#if UNITY_EDITOR
    // This is where we create an instance to be viewable
    // in scene view while the game is not running.
    void OnDrawGizmos()
    {
        if ( !Application.isPlaying )
        {
            if ( somePrefab != null && instance == null )
            {
                InstantiateEditorPrefab();
            }
            else if ( somePrefab == null && instance != null )
            {
                DestroyImmediate( instance );
                instance = null;
            }
        }
    }

    private void InstantiateEditorPrefab()
    {
        instance = Instantiate( somePrefab, transform.position, transform.rotation ) as GameObject;
        instance.transform.parent = transform;
        instance.transform.localScale = Vector3.one;

        // This will make Unity not include the object in a build.
        // "EditorOnly" is a special tag recognized by Unity.
        instance.tag = "EditorOnly";
    }
#endif

}