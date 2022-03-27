using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MeshRenderer reticle;
    public Material[] colors;
    public float switchDuration = 5f;
    public float switchTime;
    public static int crosshairColorIndex;
    
    void Start()
    {
        switchTime = switchDuration;
        
        crosshairColorIndex = Random.Range(0, colors.Length);
        reticle.material = colors[crosshairColorIndex];
    }

    
    void Update()
    {
        if (Time.time > switchTime)
        {
            crosshairColorIndex = Random.Range(0, colors.Length);
            reticle.material = colors[crosshairColorIndex];
            
            switchTime = Time.time + switchDuration;
        }
    }
}
