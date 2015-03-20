using UnityEngine;
using System.Collections;

public class PlayerShootingMechanic : MonoBehaviour
{
    public UltimateJoystick rightJoystick;

    public Gun gun1;
    public Gun gun2;

    private float mShootDelay = 0.1f;
    private float mShootTimer = 0.0f;

    void Start()
    {
        GameUtils.Assert( rightJoystick );
        GameUtils.Assert( gun1 );
        GameUtils.Assert( gun2 );
    }

    void Update()
    {
        // shooting logic
        if ( rightJoystick.JoystickPosition.sqrMagnitude > 0.01f )
        {
            mShootTimer += Time.deltaTime;
            if ( mShootTimer > mShootDelay )
            {
                mShootTimer = 0.0f;
                ShootGun( rightJoystick.JoystickPosition );
            }
        }
    }


    void ShootGun( Vector2 joystickVector )
    {
        Vector2 normalizedDirection = joystickVector.normalized;
        Vector2 randomSpread = new Vector2( Random.Range( -0.04f, 0.04f ), Random.Range( -0.04f, 0.04f ) );
        gun1.ShootGun( normalizedDirection.Rotate( -1.0f ) + randomSpread );
        gun2.ShootGun( normalizedDirection.Rotate( 1.0f ) + randomSpread );
    }
}