using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    public Camera cameraFacing;
    
    // ---------------------------------------------------------------------
    void Start()
    {
        
    }

    // ---------------------------------------------------------------------
    void Update()
    {
        
        RaycastHit hit;
        float distance;
        if(Physics.Raycast (new Ray(cameraFacing.transform.position, cameraFacing.transform.rotation * Vector3.forward),
            out hit))
        {
            distance = hit.distance;
        }
        else
        {
            distance = cameraFacing.farClipPlane * 0.95f;
        }
        
        transform.position = cameraFacing.transform.position + cameraFacing.transform.rotation * Vector3.forward * distance;
        transform.LookAt(cameraFacing.transform.position);
        transform.Rotate(0,180,0);
        transform.localScale = Vector3.one * distance / 10f;
    }
}
