using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RocketProjectile : MonoBehaviour
{
    public GameObject playerController;
    private float _timer = 0f;
    private float _t = 0f;
    private bool _exploding;
    private Vector3 _shootDirection;
    public float radius = 300f;
    public GameObject explosionPrefab;
    void Start()
    {
        playerController = GameObject.Find("PlayerController");
    }

    private void Update()
    {
        
        _timer += Time.deltaTime;
        Debug.Log(_timer);
        if (_timer > 5 && !_exploding)
        {
            Debug.Log("Rocket Projectile Exploding");
            Explosion();
        }
        
    }
    
    void Explosion()
    {
        _exploding = true;

        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(explosion,4f);

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
        Destroy(gameObject);
    }

}
