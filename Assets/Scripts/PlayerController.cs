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
    public GameObject editorCrosshair;

    public IEnumerator powerUpLaserCoroutine, freezeCoroutine;

    private float _goalBalloonCurrentScale = 1f;
    public float goalBalloonTargetScale = 200f;
    private bool _pu_laserActive = false;
    private bool _pu_rocketActive = false;

    private void Awake()
    {
        _goalBalloonCurrentScale = goalBalloon.transform.localScale.x;
        powerUpLaserPrimary.SetActive(false);
        powerUpLaserSecondary.SetActive(false);

        if (!Application.isEditor)
        {
            editorCrosshair.gameObject.SetActive(false);
        }
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

            if (_pu_rocketActive)
            {
                
            }
        }

     void ShootRocket()
     {
         
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
                     RegularBalloonHit(hit);
                 }
                 if (hit.collider.gameObject.GetComponent<PU_LaserBalloon>() != null)
                 {
                     LaserBalloonHit(hit);
                 }

                 if (hit.collider.gameObject.GetComponent<PU_MultiBalloon>() != null)
                 {
                     MultiBalloonHit(hit);
                 }

                 if (hit.collider.gameObject.GetComponent<PU_FreezeBalloon>() != null)
                 {
                     FreezeBalloonHit(hit);
                 }
                 if (hit.collider.gameObject.GetComponent<PU_NumberBalloon>() != null)
                 {
                     NumberBalloonHit(hit);
                 }
                 if (hit.collider.gameObject.GetComponent<PU_RocketBalloon>() != null)
                 {
                     RocketBalloonHit(hit);
                 }

                 /*else
                 {
                     Debug.DrawRay(controller.transform.position,
                         controller.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                     Debug.Log("Did not Hit");
                 }*/
             }
         }

    void RegularBalloonHit(RaycastHit hit)
    {
        // Hit regular balloon

        var balloon = hit.collider.gameObject.GetComponent<Balloon>();
        balloon.HitBalloon();
        OnCorrectBalloonHit();
    }

    void RocketBalloonHit(RaycastHit hit)
    {
        // Hit rocket balloon

        var balloon = hit.collider.gameObject.GetComponent<PU_RocketBalloon>();
        balloon.HitBalloon();
        OnCorrectBalloonHit();
        
        _pu_rocketActive = true;
    }
    void NumberBalloonHit(RaycastHit hit)
    {
        // Hit number balloon
        
        var balloon = hit.collider.gameObject.GetComponent<PU_NumberBalloon>();
        balloon.HitBalloon();
        OnCorrectBalloonHit();
    }
    void LaserBalloonHit(RaycastHit hit)
    {
        //Hit laser power up balloon
                     
        var balloon = hit.collider.gameObject.GetComponent<PU_LaserBalloon>();
        print("Hit Laser Balloon");
        balloon.HitBalloon();
        OnCorrectBalloonHit();

        // Need this to help with overlapping, make sure it doesn't interfere with coroutines in the future.

        if (powerUpLaserCoroutine != null)
        {
            StopCoroutine(powerUpLaserCoroutine);
        }
        
        powerUpLaserCoroutine = PowerUpLaser();
        StartCoroutine(PowerUpLaser());
    }

    void MultiBalloonHit(RaycastHit hit)
    {
        //Hit multi balloon

        var balloon = hit.collider.gameObject.GetComponent<PU_MultiBalloon>();
        print("Hit Multi Balloon");
        balloon.HitBalloon();
        OnCorrectBalloonHit();
    }

    void FreezeBalloonHit(RaycastHit hit)
    {
        // hit freeze balloon
        
        var balloon = hit.collider.gameObject.GetComponent<PU_FreezeBalloon>();
        print("Hit Freeze Balloon");
        balloon.HitBalloon();
        OnCorrectBalloonHit();
        
        foreach (var balloons in BalloonSpawner.balloonSpawnerInstance.spawnedBalloons)
        {
            if (balloons != null)
            {
                freezeCoroutine = FreezeBalloons(balloons);
                StartCoroutine(freezeCoroutine);
            }
        }
    }

    IEnumerator FreezeBalloons(GameObject balloon)
    {
        if (balloon != null)
        {
            if (balloon.GetComponent<Rigidbody>() == null)
            {
                if (balloon.transform.GetComponentInChildren<Rigidbody>())
                {
                    balloon.transform.GetComponentInChildren<Rigidbody>().isKinematic = true;
                }
            }
            else
            {
                balloon.GetComponent<Rigidbody>().isKinematic = true;
            }
            Debug.Log("Freezing");
        }

        yield return new WaitForSeconds(3f);
        
        if(balloon!=null)
        {
            if (balloon.GetComponent<Rigidbody>() == null)
            {
                if (balloon.transform.GetComponentInChildren<Rigidbody>())
                {
                    balloon.transform.GetComponentInChildren<Rigidbody>().isKinematic = false;
                }
            }
            else
            {
                balloon.GetComponent<Rigidbody>().isKinematic = false;
            }
            Debug.Log("Unfreezing");
            freezeCoroutine = null;
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
        powerUpLaserCoroutine = null;
    }
    public void OnCorrectBalloonHit()
    {
        if (goalBalloon != null)
        {
            _goalBalloonCurrentScale += 1f;
            goalBalloon.transform.localScale = new Vector3(_goalBalloonCurrentScale, _goalBalloonCurrentScale, _goalBalloonCurrentScale);
            print("Increasing size");

            // When the player has achieved the goal size, create the final explosion
            if (goalBalloon.transform.localScale.x > goalBalloonTargetScale)
            {
                // In the future, we can add more interactivity here. Example: player is alerted the big balloon is thin, and they should try shooting it to blow it up.
                GameObject effect = Instantiate(explosion, goalBalloon.transform.position, Quaternion.identity) as GameObject;
                Destroy(goalBalloon);
            }
        }
    }
}
