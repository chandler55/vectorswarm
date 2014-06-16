using UnityEngine;
using System.Collections;

public class ToggleTutorialButton : MonoBehaviour
{
    void OnClick()
    {
        Messenger.Broadcast( Events.UIEvents.ToggleTutorialScreen );
    }
}
