using System;
using System.Collections;
using System.Collections.Generic;
using Liminal.SDK.Core;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject primaryController, secondaryController, centerEye;
    public GameObject goalBalloon;
    public GameObject explosion;
    public GameObject powerUpLaserPrimary, powerUpLaserSecondary;
    public AudioSource sfxShoot, sfxLaser;

    private float _goalBalloonScale = 1f;
    private bool _pu_laserActive = false;

    private void Awake()
    {
        _goalBalloonScale = goalBalloon.transform.localScale.x;
        powerUpLaserPrimary.SetActive(false);
        powerUpLaserSecondary.SetActive(false);
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

        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot(centerEye);
            }
        }

        if (_pu_laserActive)
        {
            Shoot(primaryController);
            Shoot(secondaryController);
        }
    }
 
    void Shoot(GameObject controller)
         {
             if (!_pu_laserActive)
             {
                 sfxShoot.Play();
             }
             
             
             RaycastHit hit;
             if (Physics.Raycast(controller.transform.position,
                 controller.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
             {
                 Debug.DrawRay(controller.transform.position,
                     controller.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                 Debug.Log("Did Hit");

                 if (hit.collider.gameObject.GetComponent<Balloon>() != null)
                 {
                     // Hit regular balloon

                     var balloon = hit.collider.gameObject.GetComponent<Balloon>();
                     balloon.HitBalloon();
                     OnCorrectBalloonHit();

                 }
                 if (hit.collider.gameObject.GetComponent<PU_LaserBalloon>() != null)
                 {
                     //Hit laser power up balloon
                     
                     var balloon = hit.collider.gameObject.GetComponent<PU_LaserBalloon>();
                     print("Hit Laser Balloon");
                     balloon.HitBalloon();
                     OnCorrectBalloonHit();

                     // Need this to help with overlapping, make sure it doesn't interfere with coroutines in the future.
                     StopAllCoroutines();

                     StartCoroutine(PowerUpLaser());
                 }

                 if (hit.collider.gameObject.GetComponent<PU_MultiBalloon>() != null)
                 {
                     //Hit laser power up balloon

                     var balloon = hit.collider.gameObject.GetComponent<PU_MultiBalloon>();
                     print("Hit Multi Balloon");
                     balloon.HitBalloon();
                     OnCorrectBalloonHit();
                 }

                 /*else
                 {
                     Debug.DrawRay(controller.transform.position,
                         controller.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                     Debug.Log("Did not Hit");
                 }*/
             }
         }

    IEnumerator PowerUpLaser()
    {
        _pu_laserActive = true;
        powerUpLaserPrimary.SetActive(true);
        powerUpLaserSecondary.SetActive(true);
        sfxLaser.Play();
        
        yield return new WaitForSeconds(5f);
        
        _pu_laserActive = false;
        powerUpLaserPrimary.SetActive(false);
        powerUpLaserSecondary.SetActive(false);
        sfxLaser.Stop();
    }
    public void OnCorrectBalloonHit()
    {
        if (goalBalloon != null)
        {
            _goalBalloonScale += 1f;
            goalBalloon.transform.localScale = new Vector3(_goalBalloonScale, _goalBalloonScale, _goalBalloonScale);
            print("Increasing size");

            // When the player has achieved the goal size, create the final explosion
            if (goalBalloon.transform.localScale.x > 115)
            {
                // In the future, we can add more interactivity here. Example: player is alerted the big balloon is thin, and they should try shooting it to blow it up.
                GameObject effect = Instantiate(explosion, goalBalloon.transform.position, Quaternion.identity) as GameObject;
                Destroy(goalBalloon);
            }
        }
    }
}
