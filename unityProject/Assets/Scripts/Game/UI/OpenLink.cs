using UnityEngine;
using System.Collections;

public class OpenLink : MonoBehaviour
{
    public string url = "";

    void OnClick()
    {
        string call = "window.open('" + url + "','_blank')";

        Application.ExternalEval( call );
    }
}
