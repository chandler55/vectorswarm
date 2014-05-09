using UnityEngine;
using System.Collections;

public class HighScoreUI : MonoBehaviour
{
    private tk2dTextMesh mTextMesh = null;
    private long mScore = 0;

    void Start()
    {
        OnHighScoreUpdated( 0 );
        mTextMesh = GetComponent<tk2dTextMesh>();

        Messenger.AddListener<long>( Events.UIEvents.HighScoreUpdated, OnHighScoreUpdated );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<long>( Events.UIEvents.HighScoreUpdated, OnHighScoreUpdated );
    }

    void Update()
    {

    }

    void OnHighScoreUpdated( long score )
    {
        mScore = score;

        if ( mTextMesh )
        {
            mTextMesh.text = GameUtils.FormatNumber( mScore ); ;
        }
    }
}
