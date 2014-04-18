using UnityEngine;
using System.Collections;

public class TriggerEvent : MonoBehaviour
{
    public string eventToTrigger;

    void OnClick()
    {
        Messenger.Broadcast( eventToTrigger );
    }
}
