using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.Messaging;
using Utilities.Singleton;

namespace Input
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        private void OnMove(InputValue inputValue)
        {
            EventManager.TriggerEvent("OnPlayerMove", new Message(this)
            {
                {"direction", inputValue.Get<Vector2>()}
            });
        }

        private void OnJump(InputValue inputValue)
        {
            EventManager.TriggerEvent("OnPlayerJump", new Message(this)
            {
                {"isJumping", inputValue.isPressed}
            });
        }
        
        private void OnRun(InputValue inputValue)
        {
            EventManager.TriggerEvent("OnPlayerRun", new Message(this)
            {
                {"isRunning", inputValue.isPressed}
            });
        }
    }
}