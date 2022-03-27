using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingMovement : MonoBehaviour
{

    public float rate = 0.05f;
    void Update()
    {
        transform.Rotate(0,rate,0);
    }
}
