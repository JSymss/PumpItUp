using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalProgressBar : MonoBehaviour
{
    private float _currentProgress, _goalProgress;
    public Image fillBar;

    private void Update()
    {
        _currentProgress = PlayerController.goalBalloonCurrentScale;
        _goalProgress = PlayerController.goalBalloonTargetScale;

        fillBar.fillAmount = (_currentProgress / _goalProgress);

        //transform.rotation = Quaternion.Euler(0,0,0);

    }
}
