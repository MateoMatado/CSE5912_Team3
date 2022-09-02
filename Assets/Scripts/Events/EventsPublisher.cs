using System;
using System.Collections.Generic;

// Code written by Dr Crawfis

namespace Team3.Events
{
    internal class EventsPublisher
    {
        // Define the events that occur in the game
        private readonly Dictionary<string, Action<object, object>> events = new Dictionary<string, Action<object, object>>();
        private readonly List<Action<string, object, object>> allSubscribers = new List<Action<string, object, object>>();
        public static EventsPublisher Instance { get; private set; }
        static EventsPublisher()
        {
            Instance = new EventsPublisher();
        }
        private EventsPublisher() { }

        public void RegisterEvent(string eventName)
        {
            if (!events.ContainsKey(eventName))
            {
                events.Add(eventName, NullCallback);
            }
        }
        public void SubscribeToEvent(string eventName, Action<object, object> callback)
        {
            // Register it in case the timing is off.
            RegisterEvent(eventName);
            if (events.ContainsKey(eventName))
                events[eventName] += callback;
        }
        public void UnsubscribeToEvent(string eventName, Action<object, object> callback)
        {
            if (events.ContainsKey(eventName))
                events[eventName] -= callback;
        }
        // Todo: Either need to pass an event string (type) around or change this signature to include it for these only.
        public void SubscribeToAllEvents(Action<string, object, object> callback)
        {
            allSubscribers.Add(callback);
        }
        public void UnsubscribeToAllEvents(Action<string, object, object> callback)
        {
            allSubscribers.Remove(callback);
        }
        public void PublishEvent(string eventName, object sender, object data)
        {
            if (events.TryGetValue(eventName, out Action<object, object> eventDelegate))
                eventDelegate(sender, data);
            foreach (var handler in allSubscribers)
                handler(eventName, sender, data);
        }
        private void NullCallback(object sender, object data)
        {
        }
    }
}
