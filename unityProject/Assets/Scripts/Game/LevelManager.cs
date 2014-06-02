using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public bool useTestLevel = false;
    public GameObject levelSegmentTest;
    public GameObject levelSegmentsRoot;

    public List<GameObject> levelSegmentPrefabs;

    private float DESTROY_PREVIOUS_SEGMENT_BUFFER = 100.0f;
    private Vector3 mNextLevelSegmentPos = Vector3.zero;
    private LevelSegment mCurrentLevelSegment = null;
    private LevelSegment mNextLevelSegment = null;
    private Transform mNextLevelSegmentTransform = null;

    void Start()
    {
        GameUtils.Assert( levelSegmentsRoot );

        Messenger.AddListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
        Messenger.AddListener( Events.GameEvents.PlayerDied, OnPlayerDied );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
        Messenger.RemoveListener( Events.GameEvents.PlayerDied, OnPlayerDied );
    }

    void Update()
    {
        if ( mNextLevelSegmentTransform )
        {
            if ( PlayerSnake.Instance.Position.y >= mNextLevelSegmentTransform.position.y + DESTROY_PREVIOUS_SEGMENT_BUFFER )
            {
                Destroy( mCurrentLevelSegment.gameObject );

                mCurrentLevelSegment = mNextLevelSegment;

                CreateNextLevelSegment();
            }
        }
    }

    GameObject GetRandomLevelSegment( int indexMax = -1 )
    {
        // choose random level segment
        int prefabIndex;
        if ( indexMax == -1 )
        {
            prefabIndex = Random.Range( 0, levelSegmentPrefabs.Count );
        }
        else
        {
            prefabIndex = Random.Range( 0, indexMax );
        }


        GameObject levelSegmentPrefab = levelSegmentPrefabs[prefabIndex];
        if ( useTestLevel && levelSegmentTest )
        {
            levelSegmentPrefab = levelSegmentTest;
        }

        return levelSegmentPrefab;
    }

    void OnNewGameStarted()
    {
        // reset level starting position
        DestroyLevelSegments();

        mNextLevelSegmentPos = new Vector3( 0, 0, 0 );
        CreateBeginningLevelSegments();
    }

    void OnPlayerDied()
    {
        //DestroyLevelSegments();
    }

    void DestroyLevelSegments()
    {
        LevelSegment[] levelSegments = levelSegmentsRoot.GetComponentsInChildren<LevelSegment>();
        foreach ( LevelSegment segment in levelSegments )
        {
            Destroy( segment.gameObject );
        }
    }

    void CreateBeginningLevelSegments()
    {
        GameObject currentSegmentGameObject = Instantiate( GetRandomLevelSegment(), mNextLevelSegmentPos, Quaternion.identity ) as GameObject;
        mCurrentLevelSegment = currentSegmentGameObject.GetComponent<LevelSegment>();

        if ( mCurrentLevelSegment )
        {
            mCurrentLevelSegment.transform.parent = levelSegmentsRoot.transform;
        }

        CreateNextLevelSegment();
    }

    void CreateNextLevelSegment()
    {
        mNextLevelSegmentPos += new Vector3( 0, GameSettings.LEVEL_SEGMENT_SIZE_Y, 0 );

        GameObject nextLevelSegment = Instantiate( GetRandomLevelSegment(), mNextLevelSegmentPos, Quaternion.identity ) as GameObject;
        mNextLevelSegment = nextLevelSegment.GetComponent<LevelSegment>();

        // rotate levels randomly
        if ( UnityEngine.Random.Range( 0, 1 ) == 0 )
        {
            //mNextLevelSegment.transform.localScale = new Vector3( -1, 0, 0 );
        }

        mNextLevelSegmentTransform = mNextLevelSegment.transform;

        if ( levelSegmentsRoot )
        {
            mNextLevelSegment.transform.parent = levelSegmentsRoot.transform;
        }
    }
}
