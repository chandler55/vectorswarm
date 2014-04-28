using UnityEngine;
using System.Collections;

public class PlayerSnake : Entity
{
    private static PlayerSnake instance;
    public static PlayerSnake Instance
    {
        get
        {
            return instance;
        }
    }

    private float mEasingAmount = 0.15f;
    private float mPlayerNormalSpeed = 0.2f;
    private float mCurrentPlayerSpeedY = 0.2f;

    private Transform mTransform = null;

    void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }

    void Start()
    {
        mTransform = gameObject.transform;
    }

    void Update()
    {
        if ( Input.GetKey( KeyCode.X ) )
        {
            mCurrentPlayerSpeedY = 1.4f;
        }
        else
        {
            mCurrentPlayerSpeedY = mPlayerNormalSpeed;
        }

        //Player Movement
        {
            /*
            // move towards accelerometer tilt
            // tilt to horizontal percentage -0.25 to 0.25
            float percentage = Mathf.InverseLerp( -0.25f, 0.25f, Input.acceleration.x );

            Vector2 tiltPos = new Vector2( Boundaries.Instance.GetPercentageToPosition( percentage ), 0 );
            Vector2 target = tiltPos - Position;
            /**/
            
            // move towards mouse
            Vector2 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );

            // lock target within screen 
            Boundaries.Instance.ClampHorizontal( ref mousePos );
            Vector2 target = mousePos - Position;

            /**/

            Velocity = target * mEasingAmount;

            // lock y velocity
            Velocity = new Vector2( Velocity.x, mCurrentPlayerSpeedY );

            Position += Velocity;
        }
    }

    void CollisionTriggered( Collider2D collider )
    {
        //ParticleSystemManager.Instance.CreatePlayerExplision( Position );
    }

    public float GetY()
    {
        return mTransform.position.y;
    }
}
