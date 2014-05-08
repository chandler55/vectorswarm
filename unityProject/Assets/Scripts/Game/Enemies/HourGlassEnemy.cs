﻿using UnityEngine;
using System.Collections;

public class HourGlassEnemy : Enemy
{
    public float movementDuration = 2.0f;

    private bool mMovingRight = false;

    void Start()
    {
        mMovingRight = Position.x > 0;
        Move();
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        Go.killAllTweensWithTarget( transform );
    }

    void OnCompleteTween( AbstractGoTween tween )
    {
        Move();
    }

    void Move()
    {
        mMovingRight = !mMovingRight;

        Vector3 newPos;
        if ( mMovingRight )
        {
            newPos = new Vector3( GameSettings.WORLD_BOUNDARY.x + GameSettings.WORLD_BOUNDARY.width, Position.y, gameObject.transform.position.z );
        }
        else
        {
            newPos = new Vector3( GameSettings.WORLD_BOUNDARY.x, Position.y, gameObject.transform.position.z );
        }

        Go.to( transform, movementDuration, new GoTweenConfig().position( newPos ) ).setOnCompleteHandler( OnCompleteTween );
    }

    public override void DestroyEnemy()
    {
        base.DestroyEnemy();
        Die();
    }

    public void Die()
    {
        ObjectPool.Recycle( this );
    }
}
