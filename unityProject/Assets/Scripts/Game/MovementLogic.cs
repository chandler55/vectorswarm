using UnityEngine;
using System.Collections;

public class MovementLogic : MonoBehaviour
{
    public UltimateJoystick joystick;
    public float    mEasingAmount = 5.0f;
    public float    mThresholdForAccelerometer = 0.04f;
    private float   mPreviousAccelerationX = 0.0f;

    private PlayerSnake mPlayerSnake;

    void Start()
    {
        GameUtils.Assert( joystick );
        mPlayerSnake = gameObject.GetComponent<PlayerSnake>();
        GameUtils.Assert( mPlayerSnake );
    }

    void Update()
    {
        // pc controls
#if UNITY_EDITOR
        if ( Debug.isDebugBuild )
        {
            Vector2 movementDirection = Vector2.zero;

            if ( Input.GetKey( KeyCode.A ) )
            {
                movementDirection.x -= 1;
            }
            if ( Input.GetKey( KeyCode.D ) )
            {
                movementDirection.x += 1;
            }
            if ( Input.GetKey( KeyCode.W ) )
            {
                movementDirection.y += 1;
            }
            if ( Input.GetKey( KeyCode.S ) )
            {
                movementDirection.y -= 1;
            }

            mPlayerSnake.Move( movementDirection );

            if ( joystick.JoystickPosition.sqrMagnitude > 0.1f )
                mPlayerSnake.Move( joystick.JoystickPosition );
        }
#else
        mPlayerSnake.Move( joystick.JoystickPosition );
#endif
    }
}
