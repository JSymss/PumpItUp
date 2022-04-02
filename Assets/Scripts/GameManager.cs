using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MeshRenderer gunPrimary, gunSecondary;
    public Material[] colors;
    public float switchDuration = 5f;
    public float switchTime;
    public static int ColorIndex;
    
    void Start()
    {
        switchTime = switchDuration;
        
        ColorIndex = Random.Range(0, colors.Length);
        gunPrimary.material = colors[ColorIndex];
        gunSecondary.material = colors[ColorIndex];
    }

    
    void Update()
    {
        if (Time.time > switchTime)
        {
            ColorIndex = Random.Range(0, colors.Length);
            gunPrimary.material = colors[ColorIndex];
            gunSecondary.material = colors[ColorIndex];
            
            switchTime = Time.time + switchDuration;
        }
    }
}
