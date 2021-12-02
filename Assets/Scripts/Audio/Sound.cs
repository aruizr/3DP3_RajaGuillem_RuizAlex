using System;
using UnityEngine;

namespace Audio
{
    [Serializable]
    public struct Sound
    {
        public AudioClip audioClip;
        [Range(0, 100)] public float volume;
        public bool loop;
    }
}