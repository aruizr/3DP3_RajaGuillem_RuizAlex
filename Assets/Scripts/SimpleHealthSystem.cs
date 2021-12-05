using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Messaging;


public class SimpleHealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;
    
    [SerializeField] private int maxLives;
    private int lives;
    
    Vector3 checkpoint_position;
    
    void OnEnable()
    {
        EventManager.StartListening("star_collected", OnStarCollected);
    }

    void OnDisable()
    {
        EventManager.StopListening("star_collected", OnStarCollected);
    }
    
    void Start()
    {
        health = maxHealth;
    }

    public void LooseHealth(int amount){
        if (health <= 0)
        {
            EventManager.TriggerEvent("player_died", new Message(this));
        }
        else
        {
            health--;
        }
    }

    private void OnStarCollected(Message msg)
    {
        if (health < maxHealth) health++;
        Debug.Log("NEW HEALTH = " + health);
    }

    public void Respawn()
    {
        lives--;
        gameObject.transform.position = checkpoint_position;
    }

    public bool CanRespawn()
    {
        return lives > 0;
    }

    public void SetCheckpoint(Vector3 _checkpoint){
        checkpoint_position = _checkpoint;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("dead-zone"))
        {
            LooseHealth(7);
        }
    }
}
