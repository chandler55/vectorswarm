using UnityEngine;
using System.Collections;

public class SoundButton : OnOffSwitch
{
    void Start()
    {
        GameUtils.Assert( offIcon );

        if ( offIcon && SaveData.current != null )
        {
            mOffIconEnabled = SaveData.current.soundOn;
            UpdateOffIcon();
        }
    }


    protected override void OnClick()
    {
        SaveData.current.soundOn = !SaveData.current.soundOn;
        mOffIconEnabled = SaveData.current.soundOn;
        SaveLoad.Save();

        UpdateOffIcon();
    }
}
