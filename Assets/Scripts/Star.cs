using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Messaging;

public class Star : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            EventManager.TriggerEvent("star_collected", new Message(this));
            Destroy(gameObject);
            Debug.Log("Star destroyed");
        }
    }
}
