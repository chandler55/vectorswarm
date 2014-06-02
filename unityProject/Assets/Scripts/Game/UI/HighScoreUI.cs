using UnityEngine;
using System.Collections;

public class HighScoreUI : MonoBehaviour
{
    private tk2dTextMesh mTextMesh = null;

    void Start()
    {
        mTextMesh = GetComponent<tk2dTextMesh>();

        Messenger.AddListener<long>( Events.UIEvents.HighScoreUpdated, OnHighScoreUpdated );

        OnHighScoreUpdated( SaveData.current.highScore );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<long>( Events.UIEvents.HighScoreUpdated, OnHighScoreUpdated );
    }

    void OnHighScoreUpdated( long score )
    {
        if ( mTextMesh )
        {
            mTextMesh.text = GameUtils.FormatNumber( score ); ;
        }
    }
}
