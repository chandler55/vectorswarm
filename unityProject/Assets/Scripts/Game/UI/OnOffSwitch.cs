using UnityEngine;
using System.Collections;

public class OnOffSwitch : MonoBehaviour
{
    public tk2dSprite offIcon = null;

    protected bool mOffIconEnabled = false;

    void Start()
    {
        
    }

    void Update()
    {
    }

    protected virtual void OnClick()
    {

    }

    protected void UpdateOffIcon()
    {
        offIcon.renderer.enabled = !mOffIconEnabled;
    }
}
