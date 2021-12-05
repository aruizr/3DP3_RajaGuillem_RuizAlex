using System;
using UnityEngine;
using Utilities.Health;
using Utilities.Messaging;

namespace Player
{
    public class PlayerHealthSystem : HealthSystem
    {
        [SerializeField] private float maxLives;
        private float currentlives;

        private Vector3 lastCheckpointPosition;

        private void Start()
        {
            currentlives = maxLives;
            lastCheckpointPosition = gameObject.transform.position;
            EventManager.TriggerEvent("OnUpdatePlayerLives", new Message(this) {{"lives", currentlives}});
            EventManager.TriggerEvent("OnUpdatePlayerHealth", new Message(this) {{"health", CurrentHealth}});
        }
        
        void OnEnable(){
            EventManager.StartListening("OnRespawn", Respawn);
        }

        void OnDisable(){
            EventManager.StopListening("OnRespawn", Respawn);
        }

        protected override void OnTakeDamage()
        {
            EventManager.TriggerEvent("OnPlayerHit", new Message(this));
            EventManager.TriggerEvent("OnUpdatePlayerHealth", new Message(this) {{"health", CurrentHealth}});
        }

        protected override void OnDie()
        {
            
            Debug.Log("DIE");
            currentlives--;
            Time.timeScale = 0f;
            EventManager.TriggerEvent("OnUpdatePlayerLives", new Message(this) {{"lives", currentlives}});
            EventManager.TriggerEvent("OnPlayerDie", new Message(this));
        }
        

        public void HealPlayer(float healAmount)
        {
            if (CurrentHealth < MaxHealth)
            {
                CurrentHealth += healAmount;
                EventManager.TriggerEvent("OnUpdatePlayerHealth", new Message(this) {{"health", CurrentHealth}});
            }
        }

        public void SetCheckpoint(Vector3 checkpointPosition){
        
            lastCheckpointPosition = checkpointPosition;
            Debug.Log("CHECKPOINT SET!");
        }

        public void Respawn(Message msg)
        {
            if (currentlives <= 0) return;
            var characterController = gameObject.GetComponent<CharacterController>();
            characterController.enabled = false;
            gameObject.transform.position = lastCheckpointPosition;
            characterController.enabled = true;
            CurrentHealth = MaxHealth;
            EventManager.TriggerEvent("OnUpdatePlayerHealth", new Message(this) {{"health", CurrentHealth}});
            EventManager.TriggerEvent("Respawn", new Message(this));
            Time.timeScale = 1f;
            
            
        }

    }
}