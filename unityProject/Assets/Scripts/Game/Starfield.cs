﻿using UnityEngine;
using System.Collections;

public class Starfield : MonoBehaviour
{
    public GameObject 	tile0;
    public GameObject 	tile1;

    public tk2dSprite tile0Sprite;
    public tk2dSprite tile1Sprite;

    public float		tileScrollingSpeed = 0.0f;

    void Start()
    {
        //Messenger.AddListener<bool>( Events.GameEvents.AfterburnerTriggered, OnAfterburnerTriggered );
    }

    void OnDestroy()
    {
        //Messenger.RemoveListener<bool>( Events.GameEvents.AfterburnerTriggered, OnAfterburnerTriggered );
    }

    void Update()
    {
        {
            Vector3 tempPosition = tile0.transform.localPosition;
            tempPosition.y += Time.deltaTime * tileScrollingSpeed;
            tile0.transform.localPosition = tempPosition;

            Vector3 tile0Position = tile0.transform.localPosition;
            tile0Position.y += tile0.GetComponentInChildren<tk2dSprite>().GetBounds().extents.y * 2.0f;
            tile1.transform.localPosition = tile0Position;
        }

        // once the 2nd tile's bottom edge reaches the bottom
        // reset the 1st tile's position back to 0
        if ( tile1.transform.localPosition.y <= 0 )
        {
            Vector3 tempPosition = tile0.transform.localPosition;
            tempPosition.y = 0;
            tile0.transform.localPosition = tempPosition;
        }
    }

    void OnAfterburnerTriggered( bool afterBurnerOn )
    {
        if ( tile0Sprite && tile1Sprite )
        {
            if ( afterBurnerOn )
            {
                tile0Sprite.SetSprite( "starfield2" );
                tile1Sprite.SetSprite( "starfield2" );
            }
            else
            {
                tile0Sprite.SetSprite( "starfield" );
                tile1Sprite.SetSprite( "starfield" );
            }
        }

    }
}
