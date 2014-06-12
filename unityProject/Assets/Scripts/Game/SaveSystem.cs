using UnityEngine;
using System.Collections;

public class SaveSystem : MonoBehaviour
{
    void Awake()
    {
        LoadGame();
    }

    void LoadGame()
    {
        if ( !SaveLoad.Load() )
        {
            // create new save data
            Debug.Log( "created new save data" );
            SaveData.current = new SaveData();
        }
    }
}
