using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guido.Arkesteijn.Runtime
{
    public class DeepLinking
    {
        public static bool Initialized { get { return Instance != null; } }
        public static DeepLinking Instance { get; private set; }

        public delegate void DeepLinkingTriggerHandler(string key, Dictionary<string, object> pairs);

        private Dictionary<string, List<DeepLinkingTriggerHandler>> dictionary = new Dictionary<string, List<DeepLinkingTriggerHandler>>();
        private DeepLinkingSettings settings;

        public static void Initialize()
        {
            Initialize(DeepLinkingSettings.DefaultSettings);
        }

        public static void Initialize(DeepLinkingSettings settings)
        {
            if(Initialized)
            {
                return;
            }

            Instance = new DeepLinking();
            Instance.settings = settings;
        }
        
        public void Subscribe(string key, DeepLinkingTriggerHandler handler)
        {
            if(dictionary.ContainsKey(key))
            {
                dictionary[key].Add(handler);
            }
            else
            {
                dictionary.Add(key, new List<DeepLinkingTriggerHandler>() { handler });
            }
        }

        public void Unsubscribe(string key, DeepLinkingTriggerHandler handler)
        {
            if(dictionary.ContainsKey(key))
            {
                dictionary[key].Remove(handler);
            }
        }

        public void Trigger(string key, Dictionary<string, object> payLoad)
        {
            if(dictionary.ContainsKey(key))
            {
                foreach (var item in dictionary[key])
                {
                    item(key, payLoad);
                }
            }
        }
    }
}