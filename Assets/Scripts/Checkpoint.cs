using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void Onco(Collider other) {
        
        if(other.gameObject.CompareTag("Player")){
            PlayerHealthSystem healtSystem = other.gameObject.GetComponent<PlayerHealthSystem>();
            healtSystem.SetCheckpoint(gameObject.transform.position);
        }
            
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player")){
           Debug.Log("PLAYEEEEEEEEEEEER");
        }
    }
}
