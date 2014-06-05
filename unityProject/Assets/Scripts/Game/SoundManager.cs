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

    public AudioSource explosionSound;
    public AudioSource bombSound;
    public AudioSource playerHitSound;
    public AudioSource afterburnerSound;
    public AudioSource getMultiplierSound;

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
    }

    public void PlaySound( Sounds sound )
    {
        switch (sound)
        {
            case Sounds.Sounds_Explosion:
                if ( explosionSound )
                {
                    explosionSound.Play();
                }
                break;
            case Sounds.Sounds_Bomb:
                if ( bombSound )
                {
                    bombSound.Play();
                }
                break;
            case Sounds.Sounds_PlayerHit:
                if ( playerHitSound )
                {
                    playerHitSound.Play();
                }
                break;
            case Sounds.Sounds_Afterburner:
                if ( afterburnerSound )
                {
                    afterburnerSound.Play();
                }
                break;
            case Sounds.Sounds_GetMultiplier:
                if ( getMultiplierSound )
                {
                    getMultiplierSound.Play();
                }
                break;
        }
        
    }
}
