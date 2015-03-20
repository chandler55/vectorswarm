using UnityEngine;
using System.Collections;

public class Point
{
    public int x, y;

    public Point( int px, int py )
    {
        x = px;
        y = py;
    }
}

public class GameUtils : MonoBehaviour
{
    private static string vowels = "AEIOU";

    public static void Assert( bool condition, string message = "assert failed" )
    {
        if ( !condition )
        {
            Debug.LogError( message );
        }
    }

    public static bool IsVowel( string letter )
    {
        return vowels.Contains( letter );
    }

    public static Vector2 RandomVector2( float minLength, float maxLength )
    {
        float theta = Random.Range( 0.0f, 360.0f ) * 2 * Mathf.PI;
        float length = Random.Range( minLength, maxLength );
        Vector2 velocity = new Vector2( length * (float)Mathf.Cos( theta ), length * (float)Mathf.Sin( theta ) );
        return velocity;
    }

    public static string FormatNumber( long number )
    {
        return string.Format( "{0:#,###0}", number );
    }
}

public static class Vector2Extension
{

    public static Vector2 Rotate( this Vector2 v, float degrees )
    {
        float sin = Mathf.Sin( degrees * Mathf.Deg2Rad );
        float cos = Mathf.Cos( degrees * Mathf.Deg2Rad );

        float tx = v.x;
        float ty = v.y;
        v.x = ( cos * tx ) - ( sin * ty );
        v.y = ( sin * tx ) + ( cos * ty );
        return v;
    }
}
