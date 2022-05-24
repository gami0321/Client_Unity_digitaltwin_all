using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    void Update()
    {
        MoveCube();
    }

    void MoveCube()
    {
        transform.position = Vector3.Lerp(transform.position, Position.Npos, Mathf.SmoothStep(0, 1, 0.1f));
    }
}
