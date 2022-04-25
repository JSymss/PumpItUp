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
    public GameObject rocketProjectile;
    public GameObject standardGunPrimary, standardGunSecondary, rocketLauncherPrimary, rocketLauncherSecondary, laserPowerUpGunPrimary, laserPowerUpGunSecondary;

    public IEnumerator powerUpLaserCoroutine, freezeCoroutine;

    public static float goalBalloonCurrentScale = 1f;
    public static float goalBalloonTargetScale = 200f;
    private bool _pu_laserActive = false;
    private bool _pu_rocketActive = false;

    private void Awake()
    {
        goalBalloonCurrentScale = goalBalloon.transform.localScale.x;
        powerUpLaserPrimary.SetActive(false);
        powerUpLaserSecondary.SetActive(false);
        
        goalBalloonCurrentScale = 13f;
        goalBalloonTargetScale = 200f;

        if (!Application.isEditor)
        {
            editorCrosshair.gameObject.SetActive(false);
        }
        
        standardGunPrimary.SetActive(true);
        standardGunSecondary.SetActive(true);
        rocketLauncherPrimary.SetActive(false);
        rocketLauncherSecondary.SetActive(false);
        laserPowerUpGunPrimary.SetActive(false);
        laserPowerUpGunSecondary.SetActive(false);
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
                        if (_pu_rocketActive)
                        {
                            ShootRocket(primaryController);
                            return;
                        }
                        Shoot(primaryController);
                    }
                    if(leftHand.GetButtonDown(VRButton.Trigger))
                    {
                        if (_pu_rocketActive)
                        {
                            ShootRocket(secondaryController);
                            return;
                        }
                        Shoot(secondaryController);
                    }
                }
            }

            if (Application.isEditor)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (_pu_rocketActive)
                    {
                        ShootRocket(centerEye);
                        return;
                    }
                    Shoot(centerEye);
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                OnCorrectBalloonHit();
            }
            
            // if a rocket powerup is hit while the laser powerup is active, cancel the laser powerup.
            if (_pu_rocketActive)
            {
                _pu_laserActive = false;
                sfxLaser.Stop();
            }
            
            if (_pu_laserActive)
            {
                Shoot(primaryController);
                Shoot(secondaryController);
            }
        }

     void ShootRocket(GameObject controller)
     {
         // Play rocket SFX
         
         standardGunPrimary.SetActive(true);
         standardGunSecondary.SetActive(true);
         rocketLauncherPrimary.SetActive(false);
         rocketLauncherSecondary.SetActive(false);

         Vector3 v;
            v.x = controller.transform.rotation.x + 90;
            v.y = controller.transform.rotation.y;
            v.z = controller.transform.rotation.z;
         var q = Quaternion.Euler(v);
         
         GameObject projectile = Instantiate(rocketProjectile, controller.transform.position, controller.transform.rotation);
         
         projectile.transform.Rotate(controller.transform.rotation.x + 90,controller.transform.rotation.y,controller.transform.rotation.z);
         
         projectile.GetComponent<Rigidbody>().AddForce(controller.transform.forward*100f);
         _pu_rocketActive = false;
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
                     RegularBalloonHit(hit.collider.gameObject);
                 }
                 if (hit.collider.gameObject.GetComponent<PU_LaserBalloon>() != null)
                 {
                     LaserBalloonHit(hit.collider.gameObject);
                 }

                 if (hit.collider.gameObject.GetComponent<PU_MultiBalloon>() != null)
                 {
                     MultiBalloonHit(hit.collider.gameObject);
                 }

                 if (hit.collider.gameObject.GetComponent<PU_FreezeBalloon>() != null)
                 {
                     FreezeBalloonHit(hit.collider.gameObject);
                 }
                 if (hit.collider.gameObject.GetComponent<PU_NumberBalloon>() != null)
                 {
                     NumberBalloonHit(hit.collider.gameObject);
                 }
                 if (hit.collider.gameObject.GetComponent<PU_RocketBalloon>() != null)
                 {
                     RocketBalloonHit(hit.collider.gameObject);
                 }

                 /*else
                 {
                     Debug.DrawRay(controller.transform.position,
                         controller.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                     Debug.Log("Did not Hit");
                 }*/
             }
         }

    public void RegularBalloonHit(GameObject hit)
    {
        // Hit regular balloon

        var balloon = hit.GetComponent<Balloon>();
        balloon.HitBalloon();
        OnCorrectBalloonHit();
    }

    public void RocketBalloonHit(GameObject hit)
    {
        // Hit rocket balloon

        var balloon = hit.GetComponent<PU_RocketBalloon>();
        balloon.HitBalloon();
        OnCorrectBalloonHit();
        
        standardGunPrimary.SetActive(false);
        standardGunSecondary.SetActive(false);
        rocketLauncherPrimary.SetActive(true);
        rocketLauncherSecondary.SetActive(true);
        
        _pu_rocketActive = true;
    }
    public void NumberBalloonHit(GameObject hit)
    {
        // Hit number balloon
        
        var balloon = hit.GetComponent<PU_NumberBalloon>();
        balloon.HitBalloon();
        OnCorrectBalloonHit();
    }
    public void LaserBalloonHit(GameObject hit)
    {
        //Hit laser power up balloon
                     
        var balloon = hit.GetComponent<PU_LaserBalloon>();
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

    public void MultiBalloonHit(GameObject hit)
    {
        //Hit multi balloon

        var balloon = hit.GetComponent<PU_MultiBalloon>();
        print("Hit Multi Balloon");
        balloon.HitBalloon();
        OnCorrectBalloonHit();
    }

    public void FreezeBalloonHit(GameObject hit)
    {
        // hit freeze balloon
        
        var balloon = hit.GetComponent<PU_FreezeBalloon>();
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
        
        standardGunPrimary.SetActive(false);
        standardGunSecondary.SetActive(false);
        rocketLauncherPrimary.SetActive(false);
        rocketLauncherSecondary.SetActive(false);
        laserPowerUpGunPrimary.SetActive(true);
        laserPowerUpGunSecondary.SetActive(true);
        sfxLaser.Play();
        
        yield return new WaitForSeconds(5f);
        
        _pu_laserActive = false;
        powerUpLaserPrimary.SetActive(false);
        powerUpLaserSecondary.SetActive(false);
        
        standardGunPrimary.SetActive(true);
        standardGunSecondary.SetActive(true);
        rocketLauncherPrimary.SetActive(false);
        rocketLauncherSecondary.SetActive(false);
        laserPowerUpGunPrimary.SetActive(false);
        laserPowerUpGunSecondary.SetActive(false);
        
        sfxLaser.Stop();
        powerUpLaserCoroutine = null;
    }
    public void OnCorrectBalloonHit()
    {
        if (goalBalloon != null)
        {
            goalBalloonCurrentScale += 0.5f;
            goalBalloon.transform.localScale = new Vector3(goalBalloonCurrentScale, goalBalloonCurrentScale, goalBalloonCurrentScale);
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
