﻿using UnityEngine;
using System.Collections;
 
public class Balloon : MonoBehaviour
	{
		public enum BalloonColor { Red, Orange, blue, Green, Purple};

		public Material red, orange, blue, green, purple, invalid;

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

		public int colorIndex;

		void Start()
		{
			// Assign a random colour
			colorIndex = Random.Range(0, 5);
			
			var balloonColor = (BalloonColor) colorIndex;
			SetColor(balloonColor);

			destructTime = Time.time + lifetime + Random.value;
			balloonRigidbody = GetComponent<Rigidbody>();
			speed = new Vector3(0, Random.Range(minSpeed, maxSpeed), 0);
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
				particleObject.transform.localScale = (this.transform.localScale/35)/2;
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
			BalloonSpawner.balloonSpawnerInstance.spawnedBalloons.Remove(this.gameObject);
			Destroy(gameObject);
		}
		public void SetColor( BalloonColor color )
		{
			GetComponentInChildren<MeshRenderer>().material = BalloonMaterial( color );
		}
		
		private Material BalloonMaterial( BalloonColor balloonColor )
		{
			switch ( balloonColor )
			{
				case BalloonColor.Red:
					return new Material(red);
				case BalloonColor.Orange:
					return new Material(orange);
				case BalloonColor.blue:
					return new Material(blue);
				case BalloonColor.Green:
					return new Material(green);
				case BalloonColor.Purple:
					return new Material(purple);
				default:
					print("Invalid Color");
					break;
			}
			print("invalid material");
			return new Material(invalid);
		}

		/* //Old colour matching code
		 public bool CheckColor(int crosshairIndex)
		{
			if (crosshairIndex == colorIndex)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		*/
	}

