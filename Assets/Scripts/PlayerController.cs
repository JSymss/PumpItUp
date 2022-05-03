using System;
using System.Collections;
using System.Collections.Generic;
using Liminal.SDK.Core;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
using Liminal.Core.Fader;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject primaryController, secondaryController, centerEye;
    public GameObject goalBalloon, goalBalloonPrefab;
    public GameObject explosion, fireworks;
    public AudioSource sfxShoot, sfxLaser;
    public GameObject editorCrosshair;
    public GameObject rocketProjectile, laserProjectile;
    public GameObject standardGunPrimary, standardGunSecondary, rocketLauncherPrimary, rocketLauncherSecondary, laserPowerUpGunPrimary, laserPowerUpGunSecondary;
    public GameObject uiCanvas;

    public IEnumerator powerUpLaserCoroutine, freezeCoroutine;

    private Vector3 explosionLocation = new Vector3(0, 100, 350);

    public static float goalBalloonCurrentScale = 1f;
    public static float goalBalloonTargetScale = 200f;
    private bool _pu_laserActive = false;
    private bool _pu_rocketActive = false;

    private Quaternion goalBalloonRotation;
    private float _timer = 0f;

    private void Awake()
    {
        goalBalloonCurrentScale = goalBalloon.transform.localScale.x;

        goalBalloonCurrentScale = 13f;
        goalBalloonTargetScale = 200f;
        goalBalloonRotation = goalBalloon.transform.rotation;

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
                    if (rightHand.GetButton(VRButton.Trigger))
                    {
                        if (_pu_laserActive)
                        {
                            LaserShoot(primaryController);
                        }
                    }

                    if (leftHand.GetButton(VRButton.Trigger))
                    {
                        if (_pu_laserActive)
                        {
                            LaserShoot(secondaryController);
                        }
                    }
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

                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if (_pu_laserActive)
                    {
                        LaserShoot(centerEye);
                        return;
                    }
                }
            }

            if (Input.GetKeyDown((KeyCode.B)))
            {
                _pu_laserActive = true;
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                OnCorrectBalloonHit();
            }
            
            // if a rocket powerup is hit while the laser powerup is active, cancel the laser powerup.
            if (_pu_rocketActive)
            {
                _pu_laserActive = false;
                laserPowerUpGunPrimary.SetActive(false);
                laserPowerUpGunSecondary.SetActive(false);
                sfxLaser.Stop();
            }
        }

     void ShootRocket(GameObject controller)
     {
         // Play rocket SFX
         SoundManager.instance.rocketLaunch.Play();
         
         standardGunPrimary.SetActive(true);
         standardGunSecondary.SetActive(true);
         rocketLauncherPrimary.SetActive(false);
         rocketLauncherSecondary.SetActive(false);

         /*Vector3 v;
            v.x = controller.transform.rotation.x + 90;
            v.y = controller.transform.rotation.y;
            v.z = controller.transform.rotation.z;
         var q = Quaternion.Euler(v);*/
         
         GameObject projectile = Instantiate(rocketProjectile, controller.transform.position, controller.transform.rotation);
         
         projectile.transform.Rotate(controller.transform.rotation.x + 90,controller.transform.rotation.y,controller.transform.rotation.z);
         
         projectile.GetComponent<Rigidbody>().AddForce(controller.transform.forward*500f);
         _pu_rocketActive = false;
     }

     void LaserShoot(GameObject controller)
     {
         if (_timer >= 0.05f)
         {
             GameObject projectile = Instantiate(laserProjectile, controller.transform.position, controller.transform.rotation);
         
             projectile.GetComponent<Rigidbody>().AddForce(controller.transform.forward*5000f);

             _timer = 0f;
         }
         else
         {
             _timer += Time.deltaTime;
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

        standardGunPrimary.SetActive(false);
        standardGunSecondary.SetActive(false);
        rocketLauncherPrimary.SetActive(false);
        rocketLauncherSecondary.SetActive(false);
        laserPowerUpGunPrimary.SetActive(true);
        laserPowerUpGunSecondary.SetActive(true);
        sfxLaser.Play();
        
        yield return new WaitForSeconds(10f);
        
        _pu_laserActive = false;

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
                GameObject charging = Instantiate(explosion, explosionLocation, Quaternion.identity) as GameObject;
                GameObject fireworks_1 = Instantiate(fireworks, explosionLocation, Quaternion.identity) as GameObject;

                StartCoroutine(SpawnFireworks());
                
                StartCoroutine(EndSplash());
                
                Destroy(goalBalloon);
            }
        }
    }

    IEnumerator SpawnFireworks()
    {
        GameObject fireworks_2 = Instantiate(fireworks, explosionLocation, Quaternion.identity) as GameObject;
        yield return new WaitForSeconds(3f);
        GameObject fireworks_3 = Instantiate(fireworks, explosionLocation, Quaternion.identity) as GameObject;
    }
    
    private void EndGame()
    {
        ExperienceApp.End();
        
        var fader = ScreenFader.Instance;
        fader.FadeToBlack(3f);
        SoundManager.fadeMusicOut = true;
            
        print("End Game");
    }
    
    public void SpawnNewGoalBalloon()
    {
        GameObject gb = Instantiate(goalBalloonPrefab, new Vector3(0, 0, 350), goalBalloonRotation);
        goalBalloon = gb;
        goalBalloonCurrentScale = gb.transform.localScale.x;
    }

    IEnumerator EndSplash()
    {
        yield return new WaitForSeconds(10f);
        EndGame();
    }
}
