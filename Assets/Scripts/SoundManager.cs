using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Valve.VR.InteractionSystem;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource shoot, pop, inflate, laser, rocketPop, freezePop, laserPop, multiPop, numberPop, rocketLaunch, rocketExplosion, music, announcer;
    private float _blendTimer, pitchAlpha, _announcerTimer;
    public AudioMixer mixer;
    public static bool fadeMusicOut;
    public AudioClip[] announcerClips;

    private void Awake()
    {
        instance = this;
        fadeMusicOut = false;
    }

    private void Start()
    {
        _blendTimer = 0f;
        
        pitchAlpha = PlayerController.goalBalloonCurrentScale / PlayerController.goalBalloonTargetScale;

        music.pitch = Mathf.Lerp(1f, 1.1f, pitchAlpha);
        mixer.SetFloat("p", 1/Mathf.Lerp(1f, 1.1f, pitchAlpha));
    }

    private void Update()
    {
        if (_announcerTimer >= 20)
        {
            announcer.clip = announcerClips[Random.Range(0, announcerClips.Length)];
            announcer.Play();

            _announcerTimer = 0f;
        }

        _announcerTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        _blendTimer += Time.deltaTime;
        if (_blendTimer > 0.5f)
        {
            pitchAlpha = PlayerController.goalBalloonCurrentScale / PlayerController.goalBalloonTargetScale;

            music.pitch = Mathf.Lerp(1f, 1.1f, pitchAlpha);
            mixer.SetFloat("p", 1/Mathf.Lerp(1f, 1.1f, pitchAlpha));
        }
        if (fadeMusicOut)
        {
            mixer.GetFloat("v", out float currentVol);
            if (currentVol > -80)
            {
                mixer.SetFloat("v", currentVol-.1f);
                print("fading");
            }
        }
    }
    
}
