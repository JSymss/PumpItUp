using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject centerEye;
    public GameObject goalBalloon;
    public GameObject explosion;
    public AudioSource sfxShoot;
    
    private float goalBalloonScale = 1f;
    
    
    public delegate void HitCorrectBalloon();

    public static event HitCorrectBalloon OnHit;

    private void Start()
    {
        goalBalloonScale = goalBalloon.transform.localScale.x;
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
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            OnCorrectBalloonHit();
        }
    }

    void Shoot()
    {
        sfxShoot.Play();
        
        RaycastHit hit;
        if (Physics.Raycast(centerEye.transform.position, centerEye.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(centerEye.transform.position, centerEye.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");

            if (hit.collider.gameObject.GetComponent<Balloon>() != null)
            {
                var balloon = hit.collider.gameObject.GetComponent<Balloon>();
                if (balloon.CheckColor(GameManager.crosshairColorIndex))
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
            Debug.DrawRay(centerEye.transform.position, centerEye.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }
}
