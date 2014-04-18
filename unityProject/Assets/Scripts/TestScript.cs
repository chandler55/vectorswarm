using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour 
{

	void Start ()
    {
       // LeanTween.move(gameObject, new Vector2(1, 1), 1.0f).setEase(LeanTweenType.easeInBounce);
	}
	
	void Update () 
    {
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.A))
        {
            //Test();
        }
#endif
	}

    public void Test()
    {
        Debug.Log("test");
        TextAsset text = Resources.Load("enable1", typeof(TextAsset)) as TextAsset;
        string[] lines = text.text.Split('\n');
        Debug.Log(lines.Length);
    }

    void TestTween()
    {
       // LeanTween.move(gameObject, new Vector2(UnityEngine.Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f)), 1.0f).setEase(LeanTweenType.easeInBounce);
        //LeanTween.scale(gameObject, new Vector3(UnityEngine.Random.RandomRange(1, 2.0f), UnityEngine.Random.RandomRange(1, 2.0f), UnityEngine.Random.RandomRange(1, 2.0f)), 0.2f);
        //LeanTween.alpha(gameObject, 0.0f, 1.0f);
    }
}
