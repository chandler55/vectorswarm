using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelRandomSelection
{
    public bool includeSegmentsLevel1 = false;
    public bool includeSegmentsLevel2 = false;
    public bool includeSegmentsLevel3 = false;
    public bool includeSegmentsLevel4 = false;
    public bool includeSegmentsLevel5 = false;
    public bool includeSegmentsLevel6 = false;
}

public class LevelManager : MonoBehaviour
{
    public bool useTestLevel = false;
    public GameObject levelSegmentTest;
    public GameObject levelSegmentsRoot;

    public List<GameObject> levelSegmentPrefabs1;
    public List<GameObject> levelSegmentPrefabs2;
    public List<GameObject> levelSegmentPrefabs3;
    public List<GameObject> levelSegmentPrefabs4;
    public List<GameObject> levelSegmentPrefabs5;
    public List<GameObject> levelSegmentPrefabs6;

    public List<LevelRandomSelection> levelSelection;

    private float DESTROY_PREVIOUS_SEGMENT_BUFFER = 50.0f;
    private Vector3 mNextLevelSegmentPos = Vector3.zero;
    private LevelSegment mCurrentLevelSegment = null;
    private LevelSegment mNextLevelSegment = null;
    private Transform mNextLevelSegmentTransform = null;

    private int mCurrentLevel = 0;

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
                Entity[] entities = mCurrentLevelSegment.gameObject.GetComponentsInChildren<Entity>();
                foreach ( Entity entity in entities )
                {
                    entity.SendMessage( "Die" );
                }

                Destroy( mCurrentLevelSegment.gameObject );

                mCurrentLevelSegment = mNextLevelSegment;

                CreateNextLevelSegment();
            }
        }
    }

    GameObject GetRandomLevelSegment()
    {
        LevelRandomSelection selection;
        if ( mCurrentLevel < levelSelection.Count )
        {
            selection = levelSelection[mCurrentLevel];
        }
        else
        {
            selection = levelSelection[levelSelection.Count - 1];
        }

        mCurrentLevel++;

        List<List<GameObject>> listPrefabs = new List<List<GameObject>>();
        if ( selection.includeSegmentsLevel1 )
        {
            listPrefabs.Add( levelSegmentPrefabs1 );
        }
        if ( selection.includeSegmentsLevel2 )
        {
            listPrefabs.Add( levelSegmentPrefabs2 );
        }
        if ( selection.includeSegmentsLevel3 )
        {
            listPrefabs.Add( levelSegmentPrefabs3 );
        }
        if ( selection.includeSegmentsLevel4 )
        {
            listPrefabs.Add( levelSegmentPrefabs4 );
        }
        if ( selection.includeSegmentsLevel5 )
        {
            listPrefabs.Add( levelSegmentPrefabs5 );
        }
        if ( selection.includeSegmentsLevel6 )
        {
            listPrefabs.Add( levelSegmentPrefabs6 );
        }

        int randomPrefabSelection = Random.Range( 0, listPrefabs.Count );

        List<GameObject> currentList = listPrefabs[randomPrefabSelection];

        // choose random level segment
        int prefabIndex;
        prefabIndex = Random.Range( 0, currentList.Count );

        GameObject levelSegmentPrefab = currentList[prefabIndex];
        if ( useTestLevel && levelSegmentTest )
        {
            levelSegmentPrefab = levelSegmentTest;
        }

        return levelSegmentPrefab;
    }

    void OnNewGameStarted()
    {
        mCurrentLevel = 0;

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
