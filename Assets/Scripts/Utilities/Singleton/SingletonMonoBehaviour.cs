using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Singleton
{
    public abstract class SingletonMonoBehaviour<T> : ExtendedMonoBehaviour where T : Component
    {
        private static T _instance;
        private static readonly object Lock = new object();

        public static bool IsApplicationQuitting { get; private set; }

        public static T Instance
        {
            get
            {
                if (IsApplicationQuitting) return null;

                lock (Lock)
                {
                    if (_instance) return _instance;

                    var instances = FindObjectsOfType<T>();

                    _instance = instances.Length switch
                    {
                        0 => new GameObject($"{typeof(T)}").AddComponent<T>(),
                        1 => instances[0],
                        _ => DestroyInstances(instances)
                    };

                    (_instance as SingletonMonoBehaviour<T>)?.OnInit();

                    DontDestroyOnLoad(_instance);

                    return _instance;
                }
            }
        }

        private void OnApplicationQuit()
        {
            IsApplicationQuitting = true;
        }

        private static T DestroyInstances(IReadOnlyList<T> instances)
        {
            Debug.LogWarning(
                $"[SingletonMonoBehaviour<{typeof(T)}>] There should never be more than one Singleton of type {typeof(T)} in the scene, but {instances.Count} were found. The first instance found will be used, and all others will be destroyed.");
            for (var i = 1; i < instances.Count; i++) Destroy(instances[i]);
            return instances[0];
        }

        protected virtual void OnInit()
        {
        }
    }
}