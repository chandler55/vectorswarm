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

        Messenger.AddListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
        Messenger.AddListener( Events.GameEvents.PlayerMoved, OnPlayerMove );

    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
        Messenger.RemoveListener( Events.GameEvents.PlayerMoved, OnPlayerMove );
    }

    void OnNewGameStarted()
    {
        FollowPlayerPositionUpdate();
    }

    void OnPlayerMove()
    {
        FollowPlayerPositionUpdate();
    }

    public void FollowPlayerPositionUpdate()
    {
        if ( mFollowPlayer )
        {
            gameObject.transform.position = new Vector3( mOriginalPosition.x, PlayerSnake.Instance.Position.y + offsetY, mOriginalPosition.z );
        }
    }
}
