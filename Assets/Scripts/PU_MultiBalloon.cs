using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_MultiBalloon : MonoBehaviour
{
    public GameObject popPrefab;
    public GameObject[] balloons;

	public float maxVelocity = 5f;
	public float radius = 2f;
	public float lifetime = 25f;
	public float scale;
	public float minScale = 500f, maxScale = 1000f, explosionForce = 100f;
	public int numberOfMultiBalloonsSpawned;
	public bool burstOnLifetimeEnd = false;

	public GameObject lifetimeEndParticlePrefab;

	private float destructTime = 0f;

	private float lastSoundTime = 0f;
	private float soundDelay = 0.2f;
	public float minVol = 0.25f;
	public float maxVol = 0.75f;

	private Rigidbody balloonRigidbody;

	private bool particlesSpawned = false;

	public AudioSource lifetimeEndSound;
	public AudioSource pop;

	public Vector3 speed = new Vector3(0,10,0);
	public float minSpeed = 10f;
	public float maxSpeed = 20f;

	void Start()
	{
		destructTime = Time.time + lifetime + Random.value;
		balloonRigidbody = GetComponent<Rigidbody>();
		speed = new Vector3(Random.Range(minSpeed, maxSpeed)/10, Random.Range(minSpeed, maxSpeed), Random.Range(minSpeed, maxSpeed)/10);
		GetComponent<ConstantForce>().force = speed;
	}
	
	void Update()
	{
		if ( ( destructTime != 0 ) && ( Time.time > destructTime ) )
		{
			if ( burstOnLifetimeEnd )
			{
				SpawnParticles( lifetimeEndParticlePrefab, lifetimeEndSound );
			}
			BalloonSpawner.balloonSpawnerInstance.spawnedBalloons.Remove(this.gameObject);
			Destroy( gameObject );
		}
	}
	
	private void SpawnParticles( GameObject particlePrefab, AudioSource sound )
	{
		// Don't do this twice
		if ( particlesSpawned )
		{
			return;
		}

		particlesSpawned = true;

		if ( particlePrefab != null )
		{
			GameObject particleObject = Instantiate( particlePrefab, transform.position, transform.rotation ) as GameObject;
			particleObject.transform.localScale = this.transform.localScale;
			particleObject.GetComponent<ParticleSystem>().Play();
			Destroy( particleObject, 2f );
		}

		if ( sound != null )
		{
			sound.volume = Random.Range(minVol, maxVol);
			sound.Play();
		}
	}
	
	void FixedUpdate()
	{
		// Slow-clamp velocity
		if ( balloonRigidbody.velocity.sqrMagnitude > maxVelocity )
		{
			balloonRigidbody.velocity *= 0.97f;
		}
	}

	void OnCollisionEnter( Collision collision )
	{
		print("Balloon hit the platform, destroying balloon");
		//Destroy(gameObject);
	}

	public void HitBalloon()
	{
		print("You hit the balloon!");
		SpawnParticles(popPrefab,SoundManager.instance.pop);
		
		
		// Spawn multi balloons 

		for (int i = 0; i < numberOfMultiBalloonsSpawned; i++)
		{
			GameObject balloonPrefab = balloons[Random.Range(0, balloons.Length)];

			GameObject balloon = Instantiate( balloonPrefab, transform.position, balloonPrefab.transform.rotation ) as GameObject;
            scale = Random.Range(minScale, maxScale);
            balloon.transform.localScale = new Vector3( scale, (scale), scale );
            BalloonSpawner.balloonSpawnerInstance.spawnedBalloons.Add(balloon.gameObject);
            
            // Currently the rigidbodies are repelling each other, creating the explosion effect naturally. To get more control out of this,
            // I would like to figure out how to spawn them without them overlapping each other and apply force manually with the function shown below
            
            //balloon.GetComponent<Rigidbody>().AddExplosionForce(explosionForce,transform.position,10f,1f);
		}
		BalloonSpawner.balloonSpawnerInstance.spawnedBalloons.Remove(this.gameObject);
		Destroy(gameObject);
	}
	
}
