using UnityEngine;
using System.Collections;

public class TileSize : MonoBehaviour
{
    public Vector2 GetSize()
    {
        // grab the sprite size
        Renderer r = GetComponent<tk2dSprite>().renderer;
        return r.bounds.extents * 2.0f;
    }
}
