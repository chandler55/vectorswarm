using UnityEngine;
using System.Collections;

public class ScaleOnClick : MonoBehaviour
{
    public Vector3 scale = Vector3.one;

    void OnPress()
    {
        Scalable scalable = GetComponentInChildren<Scalable>();
        if (scalable)
        {
            scalable.AddScale(scale, this);
        }
    }

    void OnRelease()
    {
        Scalable scalable = GetComponentInChildren<Scalable>();
        if (scalable)
        {
            scalable.RemoveScale(this);
        }
    }
}
