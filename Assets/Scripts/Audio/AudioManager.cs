using System.Collections.Generic;
using Codetox.Runtime.Pooling;
using UnityEngine;
using Utilities.Messaging;
using Utilities.Misc;
using Utilities.Singleton;

namespace Audio
{
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        [SerializeField] private SerializableDictionary<string, Sound> sounds;
        
        private IObjectPool<AudioSource> _audioSources;

        private void Awake()
        {
            _audioSources = new QueuePool<AudioSource>(
                () => new GameObject().AddComponent<AudioSource>(),
                null,
                null,
                sounds.Count);
        }

        private void OnEnable()
        {
            foreach (var pair in sounds)
                EventManager.StartListening(pair.Key, message =>
                {
                    var source = ((Component) message["source"]).transform;
                    var audioSource = _audioSources.Get();
                    var audioSourceTransform = audioSource.transform;
                    var sound = pair.Value;

                    audioSource.volume = sound.volume;
                    audioSource.loop = sound.loop;
                    audioSource.clip = sound.audioClip;
                    audioSourceTransform.position = source.position;
                    audioSourceTransform.parent = source;

                    audioSource.Play();
                    _audioSources.Return(audioSource);
                });
        }
    }
}