using System;
using JetBrains.Annotations;
using UnityEngine;
using Utilities.Singleton;

namespace Utilities.Messaging
{
    public class Messenger : SingletonMonoBehaviour<Messenger>
    {
        public static void Send<T>(Action<T> callback, GameObject target = null)
        {
            if (target)
            {
                var component = target.GetComponentInChildren<T>();
                if (component != null) callback?.Invoke(component);
            }
            else
            {
                if (!(FindObjectsOfType(typeof(T)) is T[] components)) return;
                foreach (var component in components) callback?.Invoke(component);
            }
        }

        public static void SendAsync<T>(Action<T> callback, GameObject target = null)
        {
            Instance.Coroutine().Invoke(() => Send(callback, target)).Run();
        }

        public static TV Request<T, TV>([NotNull] Func<T, TV> getter, [NotNull] GameObject target)
        {
            if (getter == null) throw new ArgumentNullException(nameof(getter));
            if (target == null) throw new ArgumentNullException(nameof(target));
            var component = target.GetComponent<T>() ??
                            throw new ArgumentNullException($"No component of type <{typeof(T)}> in {nameof(target)}.");
            return getter(component);
        }
    }
}