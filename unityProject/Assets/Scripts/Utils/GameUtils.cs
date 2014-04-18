using UnityEngine;
using System.Collections;

public class Point
{
    public int x, y;

    public Point(int px, int py)
    {
        x = px;
        y = py;
    }
}

public class GameUtils : MonoBehaviour 
{
    private static string vowels = "AEIOU";

	public static void Assert (bool condition, string message = "assert failed") 
	{
		if (!condition)
		{
			Debug.LogError(message);
		}
	}

    public static bool IsVowel(string letter)
    {
        return vowels.Contains(letter);
    }

}
