using UnityEngine;
using System.Collections;

public class SendCollisionTriggerUp : MonoBehaviour
{
    void OnTriggerEnter2D( Collider2D collider )
    {
        SendMessageUpwards( "CollisionTriggered", collider );
    }
}
