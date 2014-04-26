using UnityEngine;
using System.Collections;

public class Boundaries : MonoBehaviour
{
    public Transform leftBoundary;
    public Transform rightBoundary;

    private static Boundaries instance;
    public static Boundaries Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }

    public Transform GetLeftBoundary()
    {
        return leftBoundary;
    }

    public Transform GetRightBoundary()
    {
        return rightBoundary;
    }

    public void ClampHorizontal( ref Vector2 coord )
    {
        coord.x = Mathf.Clamp( coord.x, leftBoundary.position.x, rightBoundary.position.x );
    }

    public float GetPercentageToPosition( float percent )
    {
        return Mathf.Lerp( leftBoundary.position.x, rightBoundary.position.x, percent );
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
