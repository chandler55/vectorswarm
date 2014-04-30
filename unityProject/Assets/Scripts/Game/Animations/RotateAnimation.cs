using UnityEngine;
using System.Collections;

public class RotateAnimation : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.zero;
    public float rotateSpeed = 50.0f;

    void Update()
    {
        transform.Rotate( rotationAxis, Time.deltaTime * rotateSpeed );
    }
}
