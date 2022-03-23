﻿using UnityEngine;
using System.Collections;
 
public class Balloon : MonoBehaviour
	{
		public enum BalloonColor { Red, Orange, Yellow, Green, Purple};

		public Material red, orange, yellow, green, purple, invalid;

		public GameObject popPrefab;

		public float maxVelocity = 5f;

		public float lifetime = 15f;
		public bool burstOnLifetimeEnd = false;

		public GameObject lifetimeEndParticlePrefab;

		private float destructTime = 0f;

		private float lastSoundTime = 0f;
		private float soundDelay = 0.2f;
		public float minVol = 0.25f;
		public float maxVol = 0.75f;

		private Rigidbody balloonRigidbody;

		private bool particlesSpawned = false;

		private static float s_flLastDeathSound = 0f;
		
		public AudioSource lifetimeEndSound;
		public AudioSource collisionSound;

		public Vector3 speed = new Vector3(0,10,0);
		public float minSpeed = 10f;
		public float maxSpeed = 20f;

		void Start()
		{
			// Assign a random colour
			var balloonColor = (BalloonColor) Random.Range(0, 5);
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
			SpawnParticles(lifetimeEndParticlePrefab,lifetimeEndSound);
			Destroy(gameObject);
		}

		public void HitBalloon()
		{
			print("You hit the balloon!");
			SpawnParticles(popPrefab,lifetimeEndSound);
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
				case BalloonColor.Yellow:
					return new Material(yellow);
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
	}

