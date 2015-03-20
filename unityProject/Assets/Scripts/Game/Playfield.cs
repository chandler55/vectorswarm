using UnityEngine;
using System.Collections;

public class Playfield : MonoBehaviour
{
    public BoxCollider2D playfieldBox;

    private Rect boundary = new Rect();

    private static Playfield instance;
    public static Playfield Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameUtils.Assert( playfieldBox );
        boundary = new Rect( playfieldBox.bounds.min.x, playfieldBox.bounds.min.y, playfieldBox.bounds.extents.x * 2, playfieldBox.bounds.extents.y * 2 );
    }

    public Vector2 ClampBoundary( Vector2 pos )
    {
        float x = Mathf.Clamp( pos.x, boundary.xMin, boundary.xMax );
        float y = Mathf.Clamp( pos.y, boundary.yMin, boundary.yMax );
        return new Vector2( x, y );
    }

    public bool WithinBoundary( Vector2 pos )
    {
        return boundary.Contains( pos );
    }

    public Vector2 GetRandomPos()
    {
        return new Vector2( Random.Range( boundary.xMin, boundary.xMax ), Random.Range( boundary.yMin, boundary.yMax ) );
    }

    void Update()
    {

    }
}
