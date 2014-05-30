using UnityEngine;
using System.Collections;

[System.Serializable]
public class SaveData
{
    public static SaveData current;

    public int saveVersion = 0;
    public long money = 0;

    public SaveData()
    {
        
    }

}