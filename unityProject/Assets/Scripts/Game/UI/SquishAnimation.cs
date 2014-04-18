using UnityEngine;
using System.Collections;

public class SquishAnimation : MonoBehaviour
{
    public bool isEnabled = false;

    private Vector3 mOriginalScale;
    private bool    mPulsating = false;
    private int     mTweenID = -1;
    void Start()
    {
        mOriginalScale = gameObject.transform.localScale;
    }

    void Update()
    {
        if (isEnabled && !mPulsating)
        {
            mPulsating = true;

            mOriginalScale = gameObject.transform.localScale;

            Vector3 squishScale = new Vector3(mOriginalScale.x * 0.95f, mOriginalScale.y * 1.1f, mOriginalScale.z);
            GoTween tween = Go.to( gameObject.transform, 0.7f, new GoTweenConfig().scale( squishScale ).setEaseType( GoEaseType.Linear ).setIterations(-1, GoLoopType.PingPong) );
            mTweenID = tween.id;
        }

        if ( !isEnabled && mPulsating )
        {
            mPulsating = false;
            Go.tweensWithId( mTweenID );
            gameObject.transform.localScale = mOriginalScale;
        }
    }
}
