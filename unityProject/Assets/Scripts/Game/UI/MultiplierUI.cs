using UnityEngine;
using System.Collections;

public class MultiplierUI : MonoBehaviour
{
    private tk2dTextMesh textMesh = null;

    void Start()
    {
        textMesh = GetComponent<tk2dTextMesh>();

        Messenger.AddListener<int>( Events.UIEvents.MultiplierUpdated, UpdateMultiplier );
        Messenger.AddListener( Events.GameEvents.NewGameStarted, OnNewGameStart );

        UpdateMultiplier( 0 );

    }

    void OnDestroy()
    {
        Messenger.RemoveListener<int>( Events.UIEvents.MultiplierUpdated, UpdateMultiplier );
        Messenger.RemoveListener( Events.GameEvents.NewGameStarted, OnNewGameStart );
    }

    void Update()
    {

    }

    void UpdateMultiplier( int multiplier )
    {
        if (textMesh)
        {
            textMesh.text = multiplier.ToString() + "X";
        }
    }

    void OnNewGameStart()
    {
        UpdateMultiplier( 0 );
    }
}
