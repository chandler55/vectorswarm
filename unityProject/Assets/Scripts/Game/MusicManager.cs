using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioClip song1;
    public AudioClip song2;

    public AudioSource musicAudioSource;

    private int mSongToPlay = 1;
    private bool mDecreasePitchToStop = false;

    void Start()
    {
        GameUtils.Assert( musicAudioSource );
        Messenger.AddListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
        Messenger.AddListener( Events.GameEvents.PlayerDied, OnPlayerDeath );

    }

    void Update()
    {
        if ( mDecreasePitchToStop )
        {
            if ( musicAudioSource )
            {
                musicAudioSource.pitch -= Time.deltaTime;
                if ( musicAudioSource.pitch <= 0 )
                {
                    mDecreasePitchToStop = false;
                    musicAudioSource.pitch = 1;
                    musicAudioSource.Stop();
                }
            }
        }
    }

    void OnDestroy()
    {
        Messenger.RemoveListener( Events.GameEvents.NewGameStarted, OnNewGameStarted );
        Messenger.RemoveListener( Events.GameEvents.PlayerDied, OnPlayerDeath );
    }

    void OnNewGameStarted()
    {
        PlayRandomSong();
    }

    void OnPlayerDeath()
    {
        if ( musicAudioSource )
        {
            mDecreasePitchToStop = true;
            //musicAudioSource.Stop();
        }
    }

    public void PlayRandomSong()
    {
        if ( SaveData.current != null && SaveData.current.musicOn )
        {
            if ( musicAudioSource )
            {
                if ( mSongToPlay == 1 )
                {
                    musicAudioSource.clip = song1;
                    mSongToPlay = 2;
                }
                else if ( mSongToPlay == 2 )
                {
                    musicAudioSource.clip = song2;
                    mSongToPlay = 1;
                }

                musicAudioSource.Play();
            }
        }
    }
}
