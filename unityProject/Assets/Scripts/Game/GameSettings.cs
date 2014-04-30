using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour
{
    // game only supports square grids atm
    public const float INVINCIBILITY_FLASH_SPEED = 15.0f;

    public const float LEVEL_SEGMENT_SIZE_Y = 200.0f;

    public static Rect WORLD_BOUNDARY = new Rect( -16, -100000, 32, 200000 );
}
