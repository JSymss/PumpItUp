using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    public GameObject controllerFacing;
    public Camera cameraFacing;
    private Vector3 _originalScale;
    
    // ---------------------------------------------------------------------
    void Start()
    {
        _originalScale = transform.localScale;
    }

    // ---------------------------------------------------------------------
    void Update()
    {
        
        RaycastHit hit;
        float distance;
        if(Physics.Raycast (new Ray(controllerFacing.transform.position, controllerFacing.transform.rotation * Vector3.forward),
            out hit))
        {
            distance = hit.distance ;
        }
        else
        {
            distance = cameraFacing.farClipPlane * 0.95f;
        }
        
        transform.position = controllerFacing.transform.position + controllerFacing.transform.rotation * Vector3.forward * distance;
        transform.LookAt(controllerFacing.transform.position);
        transform.Rotate(0,180,0);
        if (distance < 300.0f)
        {
            distance *= 1 + 5 * Mathf.Exp(-distance);
            transform.localScale = _originalScale * distance;
        }
        else
        {
            transform.localScale = _originalScale * 600f;
        }
        
        
        //transform.localScale = _originalScale * distance;
        
        Debug.Log(distance);
    }
}
