using UnityEngine;
using System.Collections;

public class ScoreUI : MonoBehaviour
{
    private tk2dTextMesh mTextMesh = null;
    private long mScore = 0;

    void Start()
    {
        mTextMesh = GetComponent<tk2dTextMesh>();

        Messenger.AddListener<long>( Events.UIEvents.ScoreUpdated, UpdateScore );
        Messenger.AddListener( Events.GameEvents.NewGameStarted, OnNewGameStart );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<long>( Events.UIEvents.ScoreUpdated, UpdateScore );
        Messenger.RemoveListener( Events.GameEvents.NewGameStarted, OnNewGameStart );
    }

    void UpdateScore( long score )
    {
        mScore = score;

        if ( mTextMesh )
        {
            mTextMesh.text = GameUtils.FormatNumber( mScore ); ;
        }
    }

    void OnNewGameStart()
    {
        UpdateScore( 0 );
    }
}
