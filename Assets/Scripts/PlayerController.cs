using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject centerEye;

    public GameObject goalBalloon;
    private float goalBalloonScale = 1f;
    
    public delegate void HitCorrectBalloon();

    public static event HitCorrectBalloon OnHit;

    public void OnCorrectBalloonHit()
    {
        goalBalloonScale += 0.1f;
        goalBalloon.transform.localScale = new Vector3(goalBalloonScale, goalBalloonScale, goalBalloonScale);
        print("Increasing size");
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            print("Mouse 0 clicked");
            RaycastHit hit;
            if (Physics.Raycast(centerEye.transform.position, centerEye.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(centerEye.transform.position, centerEye.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");

                if (hit.collider.gameObject.GetComponent<Balloon>() != null)
                {
                    var balloon = hit.collider.gameObject.GetComponent<Balloon>();
                    balloon.HitBalloon();
                    OnCorrectBalloonHit();
                }
            }
            else
            {
                Debug.DrawRay(centerEye.transform.position, centerEye.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("Did not Hit");
            }
        }
    }
}
