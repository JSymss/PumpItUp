using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MeshRenderer gunPrimary, gunSecondary;
    public Material[] colors;
    public Material[] crosshairColors;
    public Material[] pointerColors;
    public float switchDuration = 5f;
    public float switchTime;
    public static int ColorIndex;
    public MeshRenderer reticle_R, reticle_L;
    public LineRenderer pointer_R, pointer_L;
    
    void Start()
    {
        switchTime = switchDuration;
        
        ColorIndex = Random.Range(0, colors.Length);
        gunPrimary.material = colors[ColorIndex];
        gunSecondary.material = colors[ColorIndex];
        reticle_R.material = crosshairColors[ColorIndex];
        reticle_L.material = crosshairColors[ColorIndex];
        pointer_R.material = pointerColors[ColorIndex];
        pointer_L.material = pointerColors[ColorIndex];
    }

    
    void Update()
    {
        if (Time.time > switchTime)
        {
            ColorIndex = Random.Range(0, colors.Length);
            gunPrimary.material = colors[ColorIndex];
            gunSecondary.material = colors[ColorIndex];
            reticle_R.material = crosshairColors[ColorIndex];
            reticle_L.material = crosshairColors[ColorIndex];
            pointer_R.material = pointerColors[ColorIndex];
            pointer_L.material = pointerColors[ColorIndex];
            
            switchTime = Time.time + switchDuration;
        }
    }
}
