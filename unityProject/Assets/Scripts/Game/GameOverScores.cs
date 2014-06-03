using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOverScores : MonoBehaviour
{
    private const int NUM_SCORES = 5;

    public List<tk2dTextMesh> previousScoresTextList;
    public List<tk2dTextMesh> topScoresTextList;

    void Start()
    {
        GameUtils.Assert( previousScoresTextList.Capacity == NUM_SCORES );
        GameUtils.Assert( topScoresTextList.Capacity == NUM_SCORES );

        Messenger.AddListener<long>( Events.GameEvents.PostGameOverScore, OnScorePosted );

        UpdateScoresText();
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<long>( Events.GameEvents.PostGameOverScore, OnScorePosted );
    }

    void OnScorePosted( long score )
    {
        // push new score in list
        for ( int i = NUM_SCORES - 1; i >= 1; --i )
        {
            SaveData.current.previousScores[i] = SaveData.current.previousScores[i - 1];
        }

        SaveData.current.previousScores[0] = score;

        // check high scores and rearrange accordingly
        for ( int i = 0; i < NUM_SCORES; i++ )
        {
            if ( score > SaveData.current.topScores[i] )
            {
                MoveTopScoresDown( i );
                SaveData.current.topScores[i] = score;
                break;
            }
        }

        UpdateScoresText();
    }

    void MoveTopScoresDown( int index )
    {
        for ( int i = NUM_SCORES - 1; i > index; --i )
        {
            SaveData.current.topScores[i] = SaveData.current.topScores[i - 1];
        }
    }

    public void UpdateScoresText()
    {
        for ( int i = 0; i < NUM_SCORES; i++ )
        {
            if ( previousScoresTextList.Capacity >= i )
            {
                previousScoresTextList[i].text = GameUtils.FormatNumber( SaveData.current.previousScores[i] );
            }
        }

        for ( int i = 0; i < NUM_SCORES; i++ )
        {
            if ( topScoresTextList.Capacity >= i )
            {
                topScoresTextList[i].text = GameUtils.FormatNumber( SaveData.current.topScores[i] );
            }
        }
    }
}
