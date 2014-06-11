using UnityEngine;
using System.Collections;

public class SquishingAnimation : MonoBehaviour
{
    private tk2dSprite mSprite = null;

    // stretching animation
    private bool    mShrinkingX = false;
    private Vector3 mOriginalScale;

    void Start()
    {
        mSprite = GetComponentInChildren<tk2dSprite>();

        if ( mSprite )
        {
            mOriginalScale = mSprite.transform.localScale;
            OnCompleteSquish( null );
        }
    }

    void OnDisable()
    {
        if ( mSprite )
        {
            Go.killAllTweensWithTarget( mSprite.transform );
        }
    }

    void OnCompleteSquish( AbstractGoTween tween )
    {
        mShrinkingX = !mShrinkingX;
        Vector3 squishScale;
        if ( mShrinkingX )
        {
            squishScale = new Vector3( mOriginalScale.x * 0.55f, mOriginalScale.y * 1.5f, mOriginalScale.z );
        }
        else
        {
            squishScale = new Vector3( mOriginalScale.x * 1.5f, mOriginalScale.y * 0.55f, mOriginalScale.z );
        }

        Go.to( mSprite.transform, 0.35f, new GoTweenConfig().scale( squishScale ).setEaseType( GoEaseType.Linear ).setIterations( 2, GoLoopType.PingPong ).onComplete( OnCompleteSquish ) );
    }
}
