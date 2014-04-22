using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour
{
    // game only supports square grids atm
    public const int    MAX_GRID_SIZE_X = 7;
    public const int    MAX_GRID_SIZE_Y = 7;
    public const float  TILE_WIDTH_TO_HEIGHT_RATIO_TYPING_AREA = 0.8f;

    public static Rect WORLD_BOUNDARY = new Rect( -50, -50, 100, 100 );
}
