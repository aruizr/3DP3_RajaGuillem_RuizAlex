using System;
using UnityEngine;
using Utilities.Singleton;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/Game Settings")]
public class GameSettings : SingletonScriptableObject<GameSettings>
{
    public PlayerSettings player;

    [Serializable]
    public struct PlayerSettings
    {
        public float walkingMovementSpeed;
        public float runningMovementSpeed;
        public float turnSmoothingTime;
        public float movementSmoothingTime;
        public float maxJumpHeight;
        public float minJumpHeight;
        public float jumpApexTime;
        public float jumpComboResetTime;
    }
}