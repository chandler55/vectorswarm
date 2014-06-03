﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class ObjectPool : MonoBehaviour
{
    static ObjectPool _instance;

    Dictionary<Entity, List<Entity>> objectLookup = new Dictionary<Entity, List<Entity>>();
    Dictionary<Entity, Entity> prefabLookup = new Dictionary<Entity, Entity>();

    public static void Clear()
    {
        instance.objectLookup.Clear();
        instance.prefabLookup.Clear();
    }

    public static void CreatePool<T>( T prefab, int preallocatedCount ) where T : Entity
    {
        CreatePool<T>( prefab );
        
        List<T> preallocatedList = new List<T>();

        for ( int i = 0; i < preallocatedCount; i++ )
        {
            T obj = Spawn<T>( prefab, Vector3.zero, Quaternion.identity );
            preallocatedList.Add( obj );
        }

        for ( int i = 0; i < preallocatedCount; i++ )
        {
            Recycle<T>( preallocatedList[i] );
        }
    }

    public static void CreatePool<T>( T prefab ) where T : Entity
    {
        if ( !instance.objectLookup.ContainsKey( prefab ) )
            instance.objectLookup.Add( prefab, new List<Entity>() );
    }

    public static T Spawn<T>( T prefab, Vector3 position, Quaternion rotation ) where T : Entity
    {
        if ( instance.objectLookup.ContainsKey( prefab ) )
        {
            T obj = null;
            var list = instance.objectLookup[prefab];
            if ( list.Count > 0 )
            {
                while ( obj == null && list.Count > 0 )
                {
                    obj = list[0] as T;
                    list.RemoveAt( 0 );
                }
                if ( obj != null )
                {
                    obj.transform.parent = null;
                    obj.transform.localPosition = position;
                    obj.transform.localRotation = rotation;
                    obj.gameObject.SetActive( true );
                    obj.Reset();

                    instance.prefabLookup.Add( obj, prefab );
                    return (T)obj;
                }
            }
            obj = (T)Object.Instantiate( prefab, position, rotation );
            instance.prefabLookup.Add( obj, prefab );
            return (T)obj;
        }
        else
            return (T)Object.Instantiate( prefab, position, rotation );
    }

    public static T Spawn<T>( T prefab, Vector3 position ) where T : Entity
    {
        return Spawn( prefab, position, Quaternion.identity );
    }

    public static T Spawn<T>( T prefab ) where T : Entity
    {
        return Spawn( prefab, Vector3.zero, Quaternion.identity );
    }

    public static void Recycle<T>( T obj ) where T : Entity
    {
        if ( instance.prefabLookup.ContainsKey( obj ) )
        {
            instance.objectLookup[instance.prefabLookup[obj]].Add( obj );
            instance.prefabLookup.Remove( obj );
            obj.transform.parent = instance.transform;
            obj.gameObject.SetActive( false );

#if UNITY_EDITOR
            int sum = 0;
            foreach ( KeyValuePair<Entity, List<Entity>> pair in _instance.objectLookup )
            {
                sum += pair.Value.Count;
            }
            _instance.gameObject.name = string.Format( "_ObjectPool ({0} objects)", sum );
#endif
        }
        else
            Object.Destroy( obj.gameObject );
    }

    public static int Count<T>( T prefab ) where T : Entity
    {
        if ( instance.objectLookup.ContainsKey( prefab ) )
            return instance.objectLookup[prefab].Count;
        else
            return 0;
    }

    public static ObjectPool instance
    {
        get
        {
            if ( _instance != null )
                return _instance;
            var obj = new GameObject( "_ObjectPool" );
            obj.transform.localPosition = Vector3.zero;
            _instance = obj.AddComponent<ObjectPool>();
            return _instance;
        }
    }
}

public static class ObjectPoolExtensions
{
    public static void CreatePool<T>( this T prefab ) where T : Entity
    {
        ObjectPool.CreatePool( prefab );
    }

    public static T Spawn<T>( this T prefab, Vector3 position, Quaternion rotation ) where T : Entity
    {
        return ObjectPool.Spawn( prefab, position, rotation );
    }
    public static T Spawn<T>( this T prefab, Vector3 position ) where T : Entity
    {
        return ObjectPool.Spawn( prefab, position, Quaternion.identity );
    }
    public static T Spawn<T>( this T prefab ) where T : Entity
    {
        return ObjectPool.Spawn( prefab, Vector3.zero, Quaternion.identity );
    }

    public static void Recycle<T>( this T obj ) where T : Entity
    {
        ObjectPool.Recycle( obj );
    }

    public static int Count<T>( T prefab ) where T : Entity
    {
        return ObjectPool.Count( prefab );
    }
}
