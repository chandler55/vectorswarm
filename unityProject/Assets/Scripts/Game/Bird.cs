using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{
    private static float MAX_SPEED = 20.0f;
//    private static float MAX_STEER_FORCE = 3.0f;

    public Vector2 Position
    {
        get
        {
            return gameObject.transform.localPosition;
        }
        set
        {
            gameObject.transform.localPosition = value;
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

    private float mMaxSpeed = MAX_SPEED;
    public float MaxSpeed
    {
        get { return mMaxSpeed; }
        set { mMaxSpeed = value; }
    }

    private float mMaxSteerForce = 3f;
    public float MaxSteerForce
    {
        get { return mMaxSteerForce; }
        set { mMaxSteerForce = value; }
    }

    private bool mDestroy = false;

    void Start()
    {

    }

    void Update()
    {
        if ( mDestroy )
        {
            ParticleSystemManager.Instance.CreateEnemyExplosion( Position );
            BirdManager.Instance.DestroyBird( this );
        }
    }

    public void Destroy()
    {
        mDestroy = true;
    }
}
