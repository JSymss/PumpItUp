using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Liminal.SDK.Core;
using Liminal.Core.Fader;

public class UICanvas : MonoBehaviour
{
    public Image panel;
    public Sprite startSplash, endSplash;
    public GameObject startButton, restartButton, exitButton, playerController;

    private void Start()
    {
        startButton.SetActive(true);
        restartButton.SetActive(false);
        exitButton.SetActive(false);
        panel.sprite = startSplash;

        gameObject.SetActive(true);
        
        var fader = ScreenFader.Instance;
        fader.FadeToClearFromBlack(3f);
    }

    public void StartGame()
    {
        print("Start Game");
        startButton.SetActive(false);
        restartButton.SetActive(true);
        exitButton.SetActive(true);
        panel.sprite = endSplash;
        
        gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        playerController.GetComponent<PlayerController>().SpawnNewGoalBalloon();
        
        startButton.SetActive(true);
        restartButton.SetActive(false);
        exitButton.SetActive(false);
        panel.sprite = startSplash;

        gameObject.SetActive(true);
    }
    
    public void EndGame()
    {
        ExperienceApp.End();
        
        var fader = ScreenFader.Instance;
        fader.FadeToBlack(3f);
            
        print("End Game");
    }
}
