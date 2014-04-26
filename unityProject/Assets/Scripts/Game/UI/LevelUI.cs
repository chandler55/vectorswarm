using UnityEngine;
using System.Collections;

public class LevelUI : MonoBehaviour
{
    private tk2dTextMesh mTextMesh = null;

    void Start()
    {
        mTextMesh = gameObject.GetComponentInChildren<tk2dTextMesh>();
    }

    void Update()
    {
        if ( PlayerSnake.Instance )
        {
            float level = PlayerSnake.Instance.GetY();
            mTextMesh.text = ( (int)( level / 10.0f ) ).ToString() + " / 1000";
        }
    }
}
