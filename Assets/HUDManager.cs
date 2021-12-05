using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Messaging;

public class HUDManager : MonoBehaviour
{

    [SerializeField] private Text coinsText;
    [SerializeField] private Text livesText;
    [SerializeField] private Image healthImage;
    
    [SerializeField] private GameObject deadScreen;

    private bool isHealthShown;
    
    void OnEnable(){
        EventManager.StartListening("coin_collected", OnCoinCollected);
        EventManager.StartListening("star_collected", OnStarCollected);
        
        EventManager.StartListening("OnUpdatePlayerHealth", OnUpdateHealth);
        EventManager.StartListening("OnUpdatePlayerLives", OnUpdatePlayerLives);
        EventManager.StartListening("OnPlayerDie", OnPlayerDie);
        EventManager.StartListening("Respawn", OnRespawn);
        
        
        
    }

    void OnDisable(){
        EventManager.StopListening("coin_collected", OnCoinCollected);
        EventManager.StopListening("star_collected", OnStarCollected);
        
        EventManager.StopListening("OnUpdatePlayerHealth", OnUpdateHealth);
        EventManager.StopListening("OnUpdatePlayerLives", OnUpdatePlayerLives);
        EventManager.StopListening("OnPlayerDie", OnPlayerDie);
        EventManager.StopListening("Respawn", OnRespawn);
    }

    private void Start()
    {
        isHealthShown = false;
        coinsText.text = "0";
        livesText.text = "0";
        healthImage.fillAmount = 0.5f;

    }

    void OnCoinCollected(Message msg)
    {
        coinsText.text = int.Parse(coinsText.text) + 1 + "";
    }
    
    void OnStarCollected(Message msg)
    {
        ShowHealth();
    }

    private void ShowHealth()
    {
        if (isHealthShown) return;
        healthImage.GetComponentInParent<Animator>().SetTrigger("triggerShow");
        isHealthShown = true;
        Invoke("HideHealth", 1.8f);
    }

    private void HideHealth()
    {
        if (!isHealthShown) return;
        healthImage.GetComponentInParent<Animator>().SetTrigger("triggerHide");
        isHealthShown = false;
    }
    
    void OnUpdateHealth(Message msg)
    {
        ShowHealth();
        var health = (float) msg["health"];
        
        if (health != 0) healthImage.fillAmount = 0.125f * health;
        else healthImage.fillAmount = 0;

    }

    void OnUpdatePlayerLives(Message msg)
    {
        var lives = (float) msg["lives"];
        livesText.text = lives+"";
    }
    
    void OnPlayerDie(Message msg)
    {
        deadScreen.SetActive(true);
        healthImage.fillAmount = 0;
    }

    void OnRespawn(Message msg)
    {
        deadScreen.SetActive(false);
    }
    
    
    
}
