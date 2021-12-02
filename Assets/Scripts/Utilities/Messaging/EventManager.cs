using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Utilities.Singleton;
using EventDictionary = System.Collections.Generic.Dictionary<string, Utilities.Messaging.EventManager.EventListener>;

namespace Utilities.Messaging
{
    public class EventManager : SingletonMonoBehaviour<EventManager>
    {
        public delegate void EventListener(Message message);

        private EventDictionary _events;

        protected override void OnInit()
        {
            _events = new EventDictionary();
        }

        public static void StartListening([NotNull] string eventName, [NotNull] EventListener listener)
        {
            if (eventName == null) throw new ArgumentNullException(nameof(eventName));
            if (listener == null) throw new ArgumentNullException(nameof(listener));
            if (!Instance) return;
            if (Instance._events.TryGetValue(eventName, out var @event))
            {
                @event += listener;
                Instance._events[eventName] = @event;
                return;
            }

            Instance._events.Add(eventName, listener);
        }

        public static void StopListening([NotNull] string eventName, [NotNull] EventListener listener)
        {
            if (eventName == null) throw new ArgumentNullException(nameof(eventName));
            if (listener == null) throw new ArgumentNullException(nameof(listener));
            if (!Instance) return;
            if (!Instance._events.TryGetValue(eventName, out var @event)) return;
            @event -= listener;
            Instance._events[eventName] = @event;
        }

        public static void TriggerEvent([NotNull] string eventName, Message message)
        {
            if (eventName == null) throw new ArgumentNullException(nameof(eventName));
            if (!Instance) return;
            if (!Instance._events.TryGetValue(eventName, out var @event)) return;
            @event?.Invoke(message);
        }

        public static async void TriggerEventAsync([NotNull] string eventName, Message message)
        {
            if (eventName == null) throw new ArgumentNullException(nameof(eventName));
            await Task.Run(() => TriggerEvent(eventName, message));
        }
    }
}