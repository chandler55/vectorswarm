using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    public Vector2 Position
    {
        get
        {
            if ( mTransformCache )
            {
                return mTransformCache.position;
            }
            else
            {
                return gameObject.transform.position;
            }
        }
        set
        {
            if ( mTransformCache )
            {
                mTransformCache.position = value;
            }
            else
            {
                gameObject.transform.position = value;
            }
        }
    }

    private Vector2 mVelocity = Vector2.zero;
    public Vector2 Velocity
    {
        get
        {
            return mVelocity;
        }
        set
        {
            mVelocity = value;
        }
    }

    private bool mIsExpired;      // true if the entity was destroyed and should be deleted
    public bool IsExpired
    {
        get { return mIsExpired; }
        set { mIsExpired = value; }
    }

    private Transform mTransformCache = null;

    void Awake()
    {
        mTransformCache = gameObject.transform;
    }

    void Update()
    {

    }

    public void UpdateRotation()
    {
        Vector2 dir = ( Position + Velocity ) - (Vector2)transform.position;
        float angle = Mathf.Atan2( dir.y, dir.x ) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis( angle, Vector3.forward );
    }

    public virtual void Reset()
    {

    }

    public virtual void CollisionTriggered( Collider2D collider )
    {

    }
}
