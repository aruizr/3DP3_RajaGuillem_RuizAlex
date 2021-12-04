using System;
using MyBox;
using UnityEngine;
using Utilities;

namespace Audio
{
    [Serializable]
    public struct Sound
    {
        public AudioClip audioClip;
        [Range(0, 100)] public float volume;
        public bool loop;
        public bool is3D;
        [ConditionalField("is3D")] public Range<float> distance;
    }
}