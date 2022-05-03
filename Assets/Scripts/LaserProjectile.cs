using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public GameObject playerController;

    public float radius = 5f;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("PlayerController");
        Destroy(this, 5f);
    }

    // Update is called once per frame
    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.gameObject.GetComponent<Balloon>() != null)
            {
                playerController.GetComponent<PlayerController>().RegularBalloonHit(nearbyObject.gameObject);
                Debug.Log("Hit Regular Balloon");
            }
            else if (nearbyObject.gameObject.GetComponent<PU_LaserBalloon>() != null)
            {
                playerController.GetComponent<PlayerController>().LaserBalloonHit(nearbyObject.gameObject);
                Debug.Log("Hit Laser Balloon");
            }
            else if (nearbyObject.gameObject.GetComponent<PU_FreezeBalloon>() != null)
            {
                playerController.GetComponent<PlayerController>().FreezeBalloonHit(nearbyObject.gameObject);
                Debug.Log("Hit Freeze Balloon");
            }
            else if (nearbyObject.gameObject.GetComponent<PU_MultiBalloon>() != null)
            {
                playerController.GetComponent<PlayerController>().MultiBalloonHit(nearbyObject.gameObject);
                Debug.Log("Hit Multi Balloon");
            }
            else if (nearbyObject.gameObject.GetComponent<PU_NumberBalloon>() != null)
            {
                playerController.GetComponent<PlayerController>().NumberBalloonHit(nearbyObject.gameObject);
                Debug.Log("Hit Number Balloon");
            }
            else if (nearbyObject.gameObject.GetComponent<PU_RocketBalloon>() != null)
            {
                playerController.GetComponent<PlayerController>().RocketBalloonHit(nearbyObject.gameObject);
                Debug.Log("Hit Rocket Balloon");
            }
            else
            {
                Debug.Log("Hit no Balloons");
            }
        }
    }
}
