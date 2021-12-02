using UnityEngine;
using Utilities.Messaging;

namespace Enemies.Goomba
{
    public class AnimationEvents : MonoBehaviour
    {
        public void OnGoombaStep()
        {
            EventManager.TriggerEvent("OnGoombaStep", new Message(this));
        }
        
        public void OnGoombaAlert()
        {
            EventManager.TriggerEvent("OnGoombaAlert", new Message(this));
        }
    }
}
