using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public static SaveData current;

    public int saveVersion = 0;

    public long highScore = 0;

    public List<long> previousScores = new List<long>( new long[] { 0, 0, 0, 0, 0 } );
    public List<long> topScores = new List<long>( new long[] { 0, 0, 0, 0, 0 } );

    public SaveData()
    {

    }

}