using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guido.Arkesteijn.DeepLink.Runtime
{
    public class DeepLink
    {
        public static bool Initialized { get { return Instance != null; } }
        public static DeepLink Instance { get; private set; }

        public static string[] Arguments { get { return Instance.arguments; } }

        public delegate void DeepLinkingTriggerHandler(string key, Dictionary<string, object> pairs);

        private Dictionary<string, List<DeepLinkingTriggerHandler>> dictionary = new Dictionary<string, List<DeepLinkingTriggerHandler>>();
        private DeepLinkSettings settings;
        private string[] arguments;

        public static void Initialize()
        {
            Initialize(DeepLinkSettings.DefaultSettings);
        }

        public static void Initialize(DeepLinkSettings settings)
        {
            if(Initialized)
            {
                return;
            }

            Instance = new DeepLink();
            Instance.settings = settings;
            Instance.arguments = Environment.GetCommandLineArgs();
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