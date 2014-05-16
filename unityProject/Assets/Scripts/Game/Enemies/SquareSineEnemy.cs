using UnityEngine;
using System.Collections;

public class SquareSineEnemy : SineEnemy
{
    void Start()
    {

    }

    protected override void CalculateVelocityY()
    {
        float timeSinceStartup = Time.realtimeSinceStartup;
        Velocity = new Vector2( Velocity.x, ( mBaselineY - Position.y ) + ( ( timeSinceStartup * 2 ) % 10 < 5 ? 10 : 0 ) );
    }
}
