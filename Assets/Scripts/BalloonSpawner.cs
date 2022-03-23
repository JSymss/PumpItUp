using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public float minSpawnTime = 5f;
    public float maxSpawnTime = 15f;
    private float nextSpawnTime;
    public float radius = 500f;
    public GameObject balloonPrefab, heartBalloon, ringBalloon, starBalloon, standardBalloon, xBalloon;

    public bool autoSpawn = true;
    public bool spawnAtStartup = true;

    private float scale;
    public float minScale = 1f;
    public float maxScale = 5f;
    public float spawnForce = 1f;
    public float ySpawnOffset = -25f;

    public Transform spawnDirectionTransform;

    void Start()
    {
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
            SpawnBalloon();
            scale = Random.Range(minScale, maxScale);
            nextSpawnTime = Random.Range( minSpawnTime, maxSpawnTime ) + Time.time;
        }
    }
    public GameObject SpawnBalloon()
    {
        switch (Random.Range(0, 5))
        {
            case 0:
                balloonPrefab = heartBalloon;
                break;
            case 1:
                balloonPrefab = ringBalloon;
                break;
            case 2:
                balloonPrefab = starBalloon;
                break;
            case 3:
                balloonPrefab = standardBalloon;
                break;
            case 4:
                balloonPrefab = xBalloon;
                break;
        }
        
        Vector3 randomPos = Random.insideUnitSphere * radius;
        randomPos.y = ySpawnOffset;

        GameObject balloon = Instantiate( balloonPrefab, randomPos, balloonPrefab.transform.rotation ) as GameObject;
        balloon.transform.localScale = new Vector3( scale, (scale), scale );
        
        if ( spawnDirectionTransform != null )
        {
            balloon.GetComponentInChildren<Rigidbody>().AddForce( spawnDirectionTransform.forward * spawnForce );
        }

        return balloon;
    }
}
