using UnityEngine;
using System.Collections;

public class RemainingLivesUI : MonoBehaviour
{
    public tk2dSprite heartOne;
    public tk2dSprite heartTwo;
    public tk2dSprite heartThree;
    public tk2dTextMesh livesText;

    void Start()
    {
        GameUtils.Assert( heartOne );
        GameUtils.Assert( heartTwo );
        GameUtils.Assert( heartThree );
        GameUtils.Assert( livesText );

        Messenger.AddListener<int>( Events.UIEvents.RemainingLivesUpdated, OnRemainingLivesUpdated );

        heartOne.renderer.enabled = false;
        heartTwo.renderer.enabled = false;
        heartThree.renderer.enabled = false;
        livesText.renderer.enabled = false;
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<int>( Events.UIEvents.RemainingLivesUpdated, OnRemainingLivesUpdated );
    }

    void OnRemainingLivesUpdated( int remainingLives )
    {
        heartOne.renderer.enabled = false;
        heartTwo.renderer.enabled = false;
        heartThree.renderer.enabled = false;
        livesText.renderer.enabled = false;

        if ( remainingLives == 0 )
        {
        }
        else if ( remainingLives == 1 )
        {
            heartOne.renderer.enabled = true;
        }
        else if ( remainingLives == 2 )
        {
            heartOne.renderer.enabled = true;
            heartTwo.renderer.enabled = true;
        }
        else if ( remainingLives == 3 )
        {
            heartOne.renderer.enabled = true;
            heartTwo.renderer.enabled = true;
            heartThree.renderer.enabled = true;
        }
        else if ( remainingLives >= 4 )
        {
            heartOne.renderer.enabled = true;
            livesText.renderer.enabled = true;
            livesText.text = remainingLives.ToString();
        }
    }
}
