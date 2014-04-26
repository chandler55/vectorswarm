using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour
{
    public float    offsetY = 0.0f;

    private bool    mFollowPlayer = true;
    private Vector3 mOriginalPosition = Vector3.zero;

    void Start()
    {
        mOriginalPosition = gameObject.transform.position;
    }

    void Update()
    {
        if ( mFollowPlayer )
        {
            gameObject.transform.position = new Vector3( mOriginalPosition.x, PlayerSnake.Instance.Position.y + offsetY, mOriginalPosition.z );
        }
    }
}
