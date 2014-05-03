using UnityEngine;
using System.Collections;

public class SendDestroySignal : MonoBehaviour
{
    public void SendDestroyEnemySignal()
    {
        SendMessageUpwards( "DestroyThisEnemy" );
    }
}
