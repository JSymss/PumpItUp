using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.Newtonsoft.Json.Utilities;
using Random = UnityEngine.Random;

public class BalloonSpawner : MonoBehaviour
{
    public float minSpawnTime = 5f;
    public float maxSpawnTime = 15f;
    private float nextSpawnTime;
    public float radius = 500f;
    public GameObject[] balloons;

    public bool autoSpawn = true;
    public bool spawnAtStartup = true;

    private float scale;
    public float minScale = 1f;
    public float maxScale = 5f;
    public float spawnForce = 1f;
    public float ySpawnOffset = -25f;

    public Transform spawnDirectionTransform;
    
    public List<GameObject> spawnedBalloons;

    public static BalloonSpawner balloonSpawnerInstance;

    private void Awake()
    {
        if (balloonSpawnerInstance == null)
        {
            balloonSpawnerInstance = this;
        }
        else if(balloonSpawnerInstance!=this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        spawnedBalloons = new List<GameObject>();
        
        if ( autoSpawn && spawnAtStartup )
        {
            SpawnBalloon();
            nextSpawnTime = Random.Range( minSpawnTime, maxSpawnTime ) + Time.time;
        }
    }

    void Update()
    {
        if ( ( Time.time > nextSpawnTime ) && autoSpawn )
        {
            scale = Random.Range(minScale, maxScale);
            SpawnBalloon();
            nextSpawnTime = Random.Range( minSpawnTime, maxSpawnTime ) + Time.time;
        }
    }
    public GameObject SpawnBalloon()
    {
        GameObject balloonPrefab = balloons[Random.Range(0, balloons.Length)];

        Vector3 randomPos = Random.insideUnitSphere * radius;
        randomPos.y = ySpawnOffset;

        GameObject balloon = Instantiate( balloonPrefab, randomPos, balloonPrefab.transform.rotation ) as GameObject;
        balloon.transform.localScale = new Vector3( scale, (scale), scale );
        spawnedBalloons.Add(balloon);
        
        if ( spawnDirectionTransform != null )
        {
            balloon.GetComponentInChildren<Rigidbody>().AddForce( spawnDirectionTransform.forward * spawnForce );
        }

        return balloon;
    }

    void FreezeBalloons()
    {
        foreach (var balloon in spawnedBalloons)
        {
            balloon.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void UnFreezeBalloons()
    {
        foreach (var balloon in spawnedBalloons)
        {
            balloon.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
