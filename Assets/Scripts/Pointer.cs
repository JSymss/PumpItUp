using System;
using System.Collections;
using System.Collections.Generic;
using Liminal.SDK.VR.EventSystems;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public float defaultLength = 5.0f;
    public VRPointerInputModule inputModule;

    private LineRenderer _lineRenderer = null;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }


    void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        // Use default or distance
        float targetLength = defaultLength;


        // Raycast
        RaycastHit hit = CreateRaycast(targetLength);


        // Default
        Vector3 endPosition = transform.position + (transform.forward * targetLength);


        // Or based on hit
        if (hit.collider != null)
        {
            endPosition = hit.point;
        }

        
        // Set position of line renderer
        _lineRenderer.SetPosition(0,transform.position);
        _lineRenderer.SetPosition(1, endPosition);
        
    }

    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, defaultLength);
        

        return hit;
    }
}
