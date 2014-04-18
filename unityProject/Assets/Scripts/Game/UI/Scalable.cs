using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scalable : MonoBehaviour
{
    public const float scalingDuration = 0.1f;

    private class ScaleToCall
    {
        public Vector3 scale = Vector3.one;
        public object scaledBy = null;

        public ScaleToCall(Vector3 scaleTo, object who)
        {
            scale = scaleTo;
            scaledBy = who;
        }
    }

    private List<ScaleToCall> mScaleStack = new List<ScaleToCall>();

    public void AddScale(Vector3 newScale, object who, float scalingDuration = -1.0f)
    {
        ScaleToCall scale = new ScaleToCall(newScale, who);
        mScaleStack.Add(scale);
        SetScale(scalingDuration);
    }

    public void RemoveScale(object who, float scalingDuration = -1.0f)
    {
        ScaleToCall scaleCaller = null;

        foreach (ScaleToCall scaleCall in mScaleStack)
        {
            if (scaleCall.scaledBy == who)
            {
                scaleCaller = scaleCall;
                break;
            }
        }

        if (scaleCaller != null)
        {
            mScaleStack.Remove(scaleCaller);
        }

        SetScale(scalingDuration);
    }

    void SetScale(float duration = -1.0f)
    {
        if (duration == -1.0f)
        {
            duration = scalingDuration;
        }

        Go.killAllTweensWithTarget( gameObject.transform );

        Vector3 totalScale = Vector3.one;
        foreach (ScaleToCall scaleNum in mScaleStack)
        {
            totalScale.Scale(scaleNum.scale);
        }

        if (duration == 0.0f)
        {
            gameObject.transform.localScale = totalScale;
        }
        else
        {
            Go.to( gameObject.transform, duration, new GoTweenConfig().scale( totalScale ) );
        }
    }
}
