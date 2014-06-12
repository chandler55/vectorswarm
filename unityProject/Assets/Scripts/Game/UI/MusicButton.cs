using UnityEngine;
using System.Collections;

public class MusicButton : OnOffSwitch
{
    void Start()
    {
        GameUtils.Assert( offIcon );

        if ( offIcon && SaveData.current != null )
        {
            mOffIconEnabled = SaveData.current.musicOn;
            UpdateOffIcon();
        }
    }


    protected override void OnClick()
    {
        SaveData.current.musicOn = !SaveData.current.musicOn;
        mOffIconEnabled = SaveData.current.musicOn;
        SaveLoad.Save();

        UpdateOffIcon();
    }
}
