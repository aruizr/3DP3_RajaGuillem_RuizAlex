using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.Messaging;
using Utilities.Singleton;

namespace Input
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnMove(InputValue inputValue)
        {
            EventManager.TriggerEvent("OnActionMove", new Message(this)
            {
                {"direction", inputValue.Get<Vector2>()}
            });
        }

        private void OnJump(InputValue inputValue)
        {
            EventManager.TriggerEvent("OnActionJump", new Message(this)
            {
                {"isJumping", inputValue.isPressed}
            });
        }
        
        private void OnRun(InputValue inputValue)
        {
            EventManager.TriggerEvent("OnActionRun", new Message(this)
            {
                {"isRunning", inputValue.isPressed}
            });
        }
        
        private void OnPunch(InputValue inputValue)
        {
            EventManager.TriggerEvent("OnActionPunch", new Message(this));
        }
    }
}