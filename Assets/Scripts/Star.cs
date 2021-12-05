using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Utilities.Messaging;

public class Star : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            var playerHealthSystem = other.gameObject.GetComponent<PlayerHealthSystem>();
            playerHealthSystem.HealPlayer(1f);
            EventManager.TriggerEvent("star_collected", new Message(this));
            Destroy(gameObject);
        }
    }
}
