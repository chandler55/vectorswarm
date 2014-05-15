﻿using UnityEngine;
using System.Collections;

public class ScoreIndicator : Entity
{
    public float duration = 1.0f;

    private Transform mTransform = null;
    private float mLifetime = 0.0f;
    private tk2dTextMesh mTextMesh = null;

    void Start()
    {

    }

    void OnEnable()
    {
        mTextMesh = GetComponent<tk2dTextMesh>();
        mTransform = transform;

        mLifetime = duration;
        mTransform.localScale = Vector3.one;
    }

    void Update()
    {
        mLifetime -= Time.deltaTime;
        if ( mLifetime <= 0 )
        {
            ObjectPool.Recycle( this );
        }
        else
        {
            float scale = mLifetime / duration;

            // delay scaling for about 0.2f;
            scale = Mathf.Min( 1.0f, scale + 0.2f );

            Vector3 newScale = Vector3.one * scale;
            mTransform.localScale = newScale;
        }
    }

    public void SetScore( long score )
    {
        if ( mTextMesh )
        {
            mTextMesh.text = GameUtils.FormatNumber( score );
        }
    }

    public void SetMultiplier( int multiplier )
    {
        if ( mTextMesh )
        {
            mTextMesh.text = GameUtils.FormatNumber( multiplier ) + "X";
        }
    }
}