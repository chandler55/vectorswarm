using UnityEngine;
using System.Collections;

public class ScreenBomb : Entity
{
    public float duration = 1.0f;
    public Vector3 maxScale = new Vector3( 25, 25, 1 );

    private float mTimer = 0.0f;
    private bool mPlaySound = true;

    void OnEnable()
    {
        mTimer = 0.0f;
        transform.localScale = Vector3.one;

        mPlaySound = true;
    }

    void Update()
    {
        if ( mPlaySound )
        {
            mPlaySound = false;
            SoundManager.Instance.PlaySound( SoundManager.Sounds.Sounds_Bomb );
        }
        mTimer += Time.deltaTime;

        transform.localScale = Vector3.Lerp( Vector3.one, maxScale, mTimer / duration );

        if ( mTimer >= duration )
        {
            Recycle();
        }
    }

    void Recycle()
    {
        ObjectPool.Recycle( this );
    }

    void OnTriggerEnter2D( Collider2D collider )
    {
        if ( collider.tag == "Enemy" )
        {
            GameObject go = collider.gameObject;
            go.SendMessage( "DestroyEnemy" );
        }
    }
}
