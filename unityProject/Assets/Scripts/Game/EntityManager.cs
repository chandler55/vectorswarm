using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EntityManager : MonoBehaviour
{
    static List<Entity> addedEntities = new List<Entity>();
    static bool isUpdating = false;
    static List<Entity> entities = new List<Entity>();

    public static int Count { get { return entities.Count; } }

    void Start()
    {

    }

    public static void Add( Entity entity )
    {
        if ( !isUpdating )
        {
            entities.Add( entity );
        }
        else
        {
            addedEntities.Add( entity );
        }
    }

    void Update()
    {
        /*
        isUpdating = true;

        foreach( var entity in entities )
        {

        }
        */

        isUpdating = false;

        foreach ( var entity in addedEntities )
        {
            entities.Add( entity );
        }

        entities = entities.Where( x => !x.IsExpired ).ToList();
    }
}
