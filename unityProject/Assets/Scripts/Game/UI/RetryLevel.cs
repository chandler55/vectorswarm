using UnityEngine;
using System.Collections;

public class RetryLevel : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    void OnClick()
    {
        Messenger.Broadcast( Events.GameEvents.RetryLevel );
    }
}
