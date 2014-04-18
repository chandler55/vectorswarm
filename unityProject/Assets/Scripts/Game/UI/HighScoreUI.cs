using UnityEngine;
using System.Collections;

public class HighScoreUI : MonoBehaviour
{
    private tk2dTextMesh mTextMesh = null;
    private int mScore = 0;

    void Start()
    {
        mTextMesh = GetComponent<tk2dTextMesh>();

        Messenger.AddListener<int>( Events.UIEvents.HighScoreUpdated, OnHighScoreUpdated );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<int>( Events.UIEvents.HighScoreUpdated, OnHighScoreUpdated );
    }

    void Update()
    {

    }

    void OnHighScoreUpdated( int score )
    {
        mScore = score;

        if ( mTextMesh )
        {
            mTextMesh.text = mScore.ToString();
        }
    }
}
