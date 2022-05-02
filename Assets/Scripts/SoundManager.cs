﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Valve.VR.InteractionSystem;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource shoot, pop, inflate, laser, rocketPop, freezePop, laserPop, multiPop, numberPop, rocketLaunch, rocketExplosion, music;
    private float _timer, pitchAlpha;
    public AudioMixer mixer;
    public static bool fadeMusicOut;

    private void Awake()
    {
        instance = this;
        fadeMusicOut = false;
    }

    private void Start()
    {
        _timer = 0f;
        
        pitchAlpha = PlayerController.goalBalloonCurrentScale / PlayerController.goalBalloonTargetScale;

        music.pitch = Mathf.Lerp(1f, 1.1f, pitchAlpha);
        mixer.SetFloat("p", 1/Mathf.Lerp(1f, 1.1f, pitchAlpha));
            
        print("BENDING");
    }

    private void FixedUpdate()
    {
        _timer += Time.deltaTime;
        if (_timer > 0.5f)
        {
            pitchAlpha = PlayerController.goalBalloonCurrentScale / PlayerController.goalBalloonTargetScale;

            music.pitch = Mathf.Lerp(1f, 1.1f, pitchAlpha);
            mixer.SetFloat("p", 1/Mathf.Lerp(1f, 1.1f, pitchAlpha));
            
            print("BENDING");
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
