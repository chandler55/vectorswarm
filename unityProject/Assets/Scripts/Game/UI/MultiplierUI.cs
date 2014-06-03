using UnityEngine;
using System.Collections;

public class MultiplierUI : MonoBehaviour
{
    private tk2dTextMesh textMesh = null;

    void Start()
    {
        textMesh = GetComponent<tk2dTextMesh>();

        Messenger.AddListener<int>( Events.UIEvents.MultiplierUpdated, UpdateMultiplier );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<int>( Events.UIEvents.MultiplierUpdated, UpdateMultiplier );
    }

    void UpdateMultiplier( int multiplier )
    {
        if (textMesh)
        {
            textMesh.text = multiplier.ToString() + "X";
        }
    }
}
