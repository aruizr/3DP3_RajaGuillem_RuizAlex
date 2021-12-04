using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Messaging;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            EventManager.TriggerEvent("coin_collected", new Message(this));
        }
    }
}
