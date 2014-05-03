using UnityEngine;
using System.Collections;

public class SendCollisionTriggerUp : MonoBehaviour
{
    void OnTriggerEnter2D( Collider2D collider )
    {
        //if ( collider )
        {
            SendMessageUpwards( "CollisionTriggered", collider );
        }
    }
}
