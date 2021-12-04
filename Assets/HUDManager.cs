using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Messaging;

public class HUDManager : MonoBehaviour
{
    void OnEnable(){
        EventManager.StartListening("coin_collected", OnCoinCollected);
        EventManager.StartListening("star_collected", OnStarCollected);
        
    }

    void OnDisable(){
        EventManager.StopListening("coin_collected", OnCoinCollected);
        EventManager.StopListening("star_collected", OnStarCollected);
    }

    void OnCoinCollected(Message msg)
    {
        
    }
    
    void OnStarCollected(Message msg)
    {
        
    }
    
    
}
