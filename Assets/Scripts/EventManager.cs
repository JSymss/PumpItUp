using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void HitCorrectBalloon();

    public static event HitCorrectBalloon OnHit;

    public void OnCorrectBalloonHit()
    {
        OnHit();
    }
}
