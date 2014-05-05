using UnityEngine;
using System.Collections;

public class SendCollisionTriggerUp : MonoBehaviour
{
    void OnTriggerEnter2D( Collider2D collider )
    {
        // todo: fix this line, doesnt make sense to check for null
        if ( collider )
        {
            SendMessageUpwards( "CollisionTriggered", collider );
        }
    }
}
