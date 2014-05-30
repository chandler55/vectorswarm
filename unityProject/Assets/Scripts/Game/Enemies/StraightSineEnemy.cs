using UnityEngine;
using System.Collections;

public class StraightSineEnemy : SineEnemy
{
    protected override void CalculateVelocityY()
    {
        float timeSinceStartup = Time.realtimeSinceStartup;
        Velocity = new Vector2( Velocity.x, Mathf.Abs( timeSinceStartup % waveHeight ) - waveHeight / 2.0f );
    }

}
