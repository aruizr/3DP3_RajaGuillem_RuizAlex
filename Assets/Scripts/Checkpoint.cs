using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        
        SimpleHealthSystem healt_system = other.gameObject.GetComponent<SimpleHealthSystem>();
        
        if(other.gameObject.tag == "Player"){
            healt_system.SetCheckpoint(gameObject.transform.position);
        }
            
    }
}
