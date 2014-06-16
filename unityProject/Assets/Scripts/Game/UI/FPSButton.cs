using UnityEngine;
using System.Collections;

public class FPSButton : MonoBehaviour
{
    public tk2dSprite fps30icon = null;
    public tk2dSprite fps60icon = null;

    void Start()
    {
        UpdateIcons();
        Application.targetFrameRate = SaveData.current.fps30On ? 30 : 60;
    }


    void OnClick()
    {
        if ( SaveData.current != null )
        {
            SaveData.current.fps30On = !SaveData.current.fps30On;
            Application.targetFrameRate = SaveData.current.fps30On ? 30 : 60;
        }

        SaveLoad.Save();
        UpdateIcons();
    }

    void UpdateIcons()
    {
        if ( fps30icon && fps60icon && SaveData.current != null )
        {
            fps30icon.renderer.enabled = SaveData.current.fps30On;
            fps60icon.renderer.enabled = !SaveData.current.fps30On;
        }
    }
}
