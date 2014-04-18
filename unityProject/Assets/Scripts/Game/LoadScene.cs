using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour 
{
    public string sceneName;

	public void LoadLevel()
    {
        Application.LoadLevel(sceneName);
    }
}
