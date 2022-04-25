using System;
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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _timer = 0f;
    }

    private void FixedUpdate()
    {
        _timer += Time.deltaTime;
        if (_timer > 1f)
        {
            pitchAlpha = PlayerController.goalBalloonCurrentScale / PlayerController.goalBalloonTargetScale;

            music.pitch = Mathf.Lerp(1f, 1.3f, pitchAlpha);
            mixer.SetFloat("p", 1/Mathf.Lerp(1f, 1.3f, pitchAlpha));
            
            print("BENDING");
        }
    }
}
