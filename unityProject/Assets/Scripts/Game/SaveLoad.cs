using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoad
{
    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create( Application.persistentDataPath + "/savedGame.gd" );
        bf.Serialize( file, SaveData.current );
        file.Close();

        Debug.Log( "game saved" );
    }

    public static bool Load()
    {
        if ( File.Exists( Application.persistentDataPath + "/savedGame.gd" ) )
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open( Application.persistentDataPath + "/savedGame.gd", FileMode.Open );
            SaveData.current = (SaveData)bf.Deserialize( file );
            file.Close();

            Debug.Log( "game loaded" );
            return true;
        }
        else
        {
            return false;
        }
    }

}