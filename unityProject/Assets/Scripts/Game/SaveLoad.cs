using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoad
{
    private static bool mEnvironmentVariablesSet = false;

    public static void Save()
    {
        SetEnvironmentVariables();

#if UNITY_WP8 || UNITY_WEBPLAYER
        PlayerPrefs.SetInt( "saveVersion", SaveData.current.saveVersion );
        PlayerPrefs.SetInt( "highScore", (int)SaveData.current.highScore );

        PlayerPrefs.SetInt( "previousScores1", (int)SaveData.current.previousScores[0] );
        PlayerPrefs.SetInt( "previousScores2", (int)SaveData.current.previousScores[1] );
        PlayerPrefs.SetInt( "previousScores3", (int)SaveData.current.previousScores[2] );
        PlayerPrefs.SetInt( "previousScores4", (int)SaveData.current.previousScores[3] );
        PlayerPrefs.SetInt( "previousScores5", (int)SaveData.current.previousScores[4] );

        PlayerPrefs.SetInt( "topScores1", (int)SaveData.current.topScores[0] );
        PlayerPrefs.SetInt( "topScores2", (int)SaveData.current.topScores[1] );
        PlayerPrefs.SetInt( "topScores3", (int)SaveData.current.topScores[2] );
        PlayerPrefs.SetInt( "topScores4", (int)SaveData.current.topScores[3] );
        PlayerPrefs.SetInt( "topScores5", (int)SaveData.current.topScores[4] );

        PlayerPrefs.SetInt( "soundOn", SaveData.current.soundOn ? 1 : 0 );
        PlayerPrefs.SetInt( "musicOn", SaveData.current.musicOn ? 1 : 0 );
        PlayerPrefs.SetInt( "fps30On", SaveData.current.fps30On ? 1 : 0 );
        PlayerPrefs.SetInt( "socialAuthenticated", SaveData.current.socialAuthenticated ? 1 : 0 );
#else
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create( Application.persistentDataPath + "/savedGame.gd" );
        bf.Serialize( file, SaveData.current );
        file.Close();
        Debug.Log( "game saved" );
#endif
    }

    public static bool Load()
    {
        SetEnvironmentVariables();
#if UNITY_WP8 || UNITY_WEBPLAYER
        if ( SaveData.current == null )
        {
            SaveData.current = new SaveData();
        }

        SaveData.current.saveVersion = PlayerPrefs.GetInt( "saveVersion", SaveData.current.saveVersion );
        SaveData.current.highScore = PlayerPrefs.GetInt( "highScore", (int)SaveData.current.highScore );

        SaveData.current.previousScores[0] = PlayerPrefs.GetInt( "previousScores1", (int)SaveData.current.previousScores[0] );
        SaveData.current.previousScores[1] = PlayerPrefs.GetInt( "previousScores2", (int)SaveData.current.previousScores[1] );
        SaveData.current.previousScores[2] = PlayerPrefs.GetInt( "previousScores3", (int)SaveData.current.previousScores[2] );
        SaveData.current.previousScores[3] = PlayerPrefs.GetInt( "previousScores4", (int)SaveData.current.previousScores[3] );
        SaveData.current.previousScores[4] = PlayerPrefs.GetInt( "previousScores5", (int)SaveData.current.previousScores[4] );

        SaveData.current.topScores[0] = PlayerPrefs.GetInt( "topScores1", (int)SaveData.current.topScores[0] );
        SaveData.current.topScores[1] = PlayerPrefs.GetInt( "topScores2", (int)SaveData.current.topScores[1] );
        SaveData.current.topScores[2] = PlayerPrefs.GetInt( "topScores3", (int)SaveData.current.topScores[2] );
        SaveData.current.topScores[3] = PlayerPrefs.GetInt( "topScores4", (int)SaveData.current.topScores[3] );
        SaveData.current.topScores[4] = PlayerPrefs.GetInt( "topScores5", (int)SaveData.current.topScores[4] );

        SaveData.current.soundOn = PlayerPrefs.GetInt( "soundOn", SaveData.current.soundOn ? 1 : 0 ) == 1;
        SaveData.current.musicOn = PlayerPrefs.GetInt( "musicOn", SaveData.current.musicOn ? 1 : 0 ) == 1;
        SaveData.current.fps30On = PlayerPrefs.GetInt( "fps30On", SaveData.current.fps30On ? 1 : 0 ) == 1;
        SaveData.current.socialAuthenticated = PlayerPrefs.GetInt( "socialAuthenticated", SaveData.current.socialAuthenticated ? 1 : 0 ) == 1;
        return true;
#else
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
#endif
    }

    static void SetEnvironmentVariables()
    {
#if !UNITY_WP8 && !UNITY_WEBPLAYER
        if ( !mEnvironmentVariablesSet )
        {
            mEnvironmentVariablesSet = true;
            System.Environment.SetEnvironmentVariable( "MONO_REFLECTION_SERIALIZER", "yes" );
        }
#endif
    }

}