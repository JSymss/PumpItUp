using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingMovement : MonoBehaviour
{

    public float rate = 0.05f;

    void FixedUpdate()
    {
        transform.Rotate(0,rate,0, Space.World);
        
        //transform.Rotate(transform.rotation.x*-0.90f,0,0, Space.World);
        //print("Rotating X");
        
        //transform.Rotate(0,0,transform.rotation.z*-0.90f, Space.World);
        //print("Rotating Z");
        
    }
}
