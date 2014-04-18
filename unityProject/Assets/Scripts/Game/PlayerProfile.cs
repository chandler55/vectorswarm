using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerProfile : Singleton<PlayerProfile>
{
    public class LevelData
    {
        public int     levelNumber = 0;
        public bool    unlocked = false;
        public int     starsEarned = 0;
        public int     highScore = 0;
    }

    private List<LevelData> mLevelData = new List<LevelData>();
    private string          mFacebookID;

    void Start()
    {
        DontDestroyOnLoad( gameObject );
    }

    void Update()
    {

    }

    void ResetProfile()
    {
        for ( int i = 0; i < 80; i++ )
        {
            LevelData levelData = new LevelData();
            levelData.levelNumber = i;

            if ( i == 0 )
            {
                levelData.unlocked = true;
            }

            mLevelData.Add( levelData );
        }
    }
}
