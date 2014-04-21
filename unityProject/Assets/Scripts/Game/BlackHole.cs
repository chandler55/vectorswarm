using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackHole : Entity
{
    public static List<BlackHole> blackHolesList = new List<BlackHole>();

    void Start()
    {
        blackHolesList.Add( this );
    }

    void OnDestroy()
    {
        blackHolesList.Remove( this );
    }

    void Update()
    {

    }
}
