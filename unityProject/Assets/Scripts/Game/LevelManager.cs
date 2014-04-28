using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{

    public List<GameObject> levelSegmentPrefabs;

    private float DESTROY_PREVIOUS_SEGMENT_BUFFER = 100.0f;
    private Vector3 mNextLevelSegmentPos = Vector3.zero;
    private GameObject mCurrentLevelSegment = null;
    private GameObject mNextLevelSegment = null;
    private Transform mNextLevelSegmentTransform = null;

    void Start()
    {
        mCurrentLevelSegment = Instantiate( levelSegmentPrefabs[0], mNextLevelSegmentPos, Quaternion.identity ) as GameObject;
        CreateNextLevelSegment();
    }

    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.C ) || Input.GetMouseButtonUp( 0 ) )
        {
            //Instantiate( levelSegmentPrefabs[0], mNextLevelSegmentPos, Quaternion.identity );
        }

        if ( PlayerSnake.Instance.Position.y >= mNextLevelSegmentTransform.position.y + DESTROY_PREVIOUS_SEGMENT_BUFFER )
        {
            Destroy( mCurrentLevelSegment );

            mCurrentLevelSegment = mNextLevelSegment;

            CreateNextLevelSegment();
        }


    }

    void CreateNextLevelSegment()
    {
        mNextLevelSegmentPos += new Vector3( 0, GameSettings.LEVEL_SEGMENT_SIZE_Y, 0 );
        mNextLevelSegment = Instantiate( levelSegmentPrefabs[0], mNextLevelSegmentPos, Quaternion.identity ) as GameObject;
        mNextLevelSegmentTransform = mNextLevelSegment.transform;
    }
}
