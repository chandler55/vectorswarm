using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public enum Sounds
    {
        Sounds_Explosion,
        Sounds_Bomb,
        Sounds_PlayerHit,
        Sounds_Afterburner,
        Sounds_GetMultiplier,
    }

    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }

    public AudioClip explosionSound;
    public AudioClip bombSound;
    public AudioClip playerHitSound;
    public AudioClip afterburnerSound;
    public AudioClip getMultiplierSound;
    public AudioSource audioSource;

    private int limitMultiplierSound = 10;

    void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }

    void Start()
    {
        GameUtils.Assert( explosionSound );
        GameUtils.Assert( bombSound );
        GameUtils.Assert( playerHitSound );
        GameUtils.Assert( afterburnerSound );
        GameUtils.Assert( getMultiplierSound );

        GameUtils.Assert( audioSource );
    }

    public void PlaySound( Sounds sound )
    {
        if ( SaveData.current != null && SaveData.current.soundOn )
        {
            switch ( sound )
            {
                case Sounds.Sounds_Explosion:
                    if ( explosionSound )
                    {
                        audioSource.PlayOneShot( explosionSound );
                    }
                    break;
                case Sounds.Sounds_Bomb:
                    if ( bombSound )
                    {
                        audioSource.PlayOneShot( bombSound );
                    }
                    break;
                case Sounds.Sounds_PlayerHit:
                    if ( playerHitSound )
                    {
                        audioSource.PlayOneShot( playerHitSound );
                    }
                    break;
                case Sounds.Sounds_Afterburner:
                    if ( afterburnerSound )
                    {
                        audioSource.PlayOneShot( afterburnerSound );
                    }
                    break;
                case Sounds.Sounds_GetMultiplier:
                    if ( getMultiplierSound )
                    {
                        audioSource.PlayOneShot( getMultiplierSound );
                    }
                    break;
            }
        }
    }
}
