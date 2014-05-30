using UnityEngine;
using System.Collections;

public class PlayerShield : MonoBehaviour
{
    public float chargeDuration = 10.0f;

    private tk2dRadialSprite sprite = null;
    private bool    mCharged = true;
    private float   mChargeValue = 1.0f;

    void Start()
    {
        sprite = GetComponent<tk2dRadialSprite>();
    }

    void Update()
    {
        if ( mCharged )
        {
        }
        else
        {
            mChargeValue += Time.deltaTime / chargeDuration;
            if ( mChargeValue >= 1 )
            {
                mChargeValue = 1.0f;
                mCharged = true;
            }

            if ( sprite )
            {
                sprite.VisibleAmount = mChargeValue;
            }
        }
    }

    public bool IsCharged()
    {
        return mCharged;
    }

    public void UseShield()
    {
        mCharged = false;
        mChargeValue = 0.0f;

        EntityDatabase.Instance.CreateEntity( EntityDatabase.EntityType.EntityType_ScreenBomb, transform.position, Quaternion.identity );
    }
}
