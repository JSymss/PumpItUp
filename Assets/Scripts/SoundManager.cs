using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource shoot, pop, inflate;

    private void Awake()
    {
        instance = this;
    }

}
