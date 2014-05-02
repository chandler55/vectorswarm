using UnityEngine;
using System.Collections;

public class RotateOnVelocity : MonoBehaviour
{
    public  GameObject  shipObject;

    public  float       smoothingTime = 0.3f;

    private float mRotation = 0.0f;
    private Entity mEntity;
    private float mVelocityOfSmoothingFunction = 0.0f;
    void Start()
    {
        mEntity = GetComponent<Entity>();
    }

    void Update()
    {
        if ( mEntity && shipObject )
        {
            mRotation = Mathf.SmoothDampAngle( mRotation, -mEntity.Velocity.x, ref mVelocityOfSmoothingFunction, smoothingTime );
            shipObject.transform.rotation = Quaternion.Euler( new Vector3( 0, mRotation, 0 ) );
        }
    }
}
