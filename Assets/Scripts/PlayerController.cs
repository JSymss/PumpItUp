using System;
using System.Collections;
using System.Collections.Generic;
using Liminal.SDK.Core;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject primaryController, secondaryController;
    public GameObject goalBalloon;
    public GameObject explosion;
    public AudioSource sfxShoot;

    private float goalBalloonScale = 1f;
    
    
    public delegate void HitCorrectBalloon();

    public static event HitCorrectBalloon OnHit;

    private void Awake()
    {
        goalBalloonScale = goalBalloon.transform.localScale.x;
        
       

    }
 void Update()
    {
        if (!Application.isEditor)
        {
            var device = VRDevice.Device;
            var rightHand = device.PrimaryInputDevice;
            var leftHand = device.SecondaryInputDevice;
        
            if (device != null)
            {
                if(rightHand.GetButtonDown(VRButton.Trigger))
                {
                    Shoot(primaryController);
                }
                if(leftHand.GetButtonDown(VRButton.Trigger))
                {
                    Shoot(secondaryController);
                }
            }
       }
    }
 
    void Shoot(GameObject controller)
         {
             sfxShoot.Play();
             
             RaycastHit hit;
             if (Physics.Raycast(controller.transform.position, controller.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
             {
                 Debug.DrawRay(controller.transform.position, controller.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                 Debug.Log("Did Hit");
     
                 if (hit.collider.gameObject.GetComponent<Balloon>() != null)
                 {
                     var balloon = hit.collider.gameObject.GetComponent<Balloon>();
                     if (balloon.CheckColor(GameManager.ColorIndex))
                     {
                         balloon.HitBalloon();
                         OnCorrectBalloonHit();
                     }
                     else
                     {
                         //balloon.HitBalloon();
                         //OnIncorrectBalloonHit();
                         print("Wrong Balloon!");
                     }
                         
                 }
             }
             else
             {
                 Debug.DrawRay(controller.transform.position, controller.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                 Debug.Log("Did not Hit");
             }
         }
    public void OnCorrectBalloonHit()
    {
        if (goalBalloon != null)
        {
            goalBalloonScale += 1f;
            goalBalloon.transform.localScale = new Vector3(goalBalloonScale, goalBalloonScale, goalBalloonScale);
            print("Increasing size");

            // When the player has achieved the goal size, create the final explosion
            if (goalBalloon.transform.localScale.x > 115)
            {
                GameObject effect = Instantiate(explosion, goalBalloon.transform.position, Quaternion.identity) as GameObject;
                Destroy(goalBalloon);
            }
        }
    }

    public void OnIncorrectBalloonHit()
    {
        if (goalBalloon != null)
        {
        }
    }
}
