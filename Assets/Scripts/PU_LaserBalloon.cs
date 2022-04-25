using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_LaserBalloon : MonoBehaviour
{
	public GameObject popPrefab;

	public float maxVelocity = 5f;

	public float lifetime = 25f;
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
		SpawnParticles(popPrefab,SoundManager.instance.laserPop);
		BalloonSpawner.balloonSpawnerInstance.spawnedBalloons.Remove(this.gameObject);
		Destroy(gameObject);
	}
	
}
