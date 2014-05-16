using UnityEngine;
using System.Collections;

public class TriangleSineEnemy : SineEnemy
{
    void Start()
    {

    }

    protected override void CalculateVelocityY()
    {
        float timeSinceStartup = Time.realtimeSinceStartup;
        Debug.Log( ( Mathf.Abs( ( 80 * timeSinceStartup ) % ( waveHeight * 16 ) - ( waveHeight * 8 ) ) ) - ( waveHeight * 4 ) );
        Velocity = new Vector2( Velocity.x, ( mBaselineY - Position.y ) + ( Mathf.Abs( ( 80 * timeSinceStartup ) % ( waveHeight * 16 ) - ( waveHeight * 8 ) ) ) - ( waveHeight * 4 ) );
    }
}
