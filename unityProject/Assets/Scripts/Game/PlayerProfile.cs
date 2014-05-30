using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerProfile : Singleton<PlayerProfile>
{
    public bool hasKillAllShield = false;

    private string          mFacebookID;

    void Start()
    {
        DontDestroyOnLoad( gameObject );
    }

    void Update()
    {

    }

    void ResetProfile()
    {

    }
}
