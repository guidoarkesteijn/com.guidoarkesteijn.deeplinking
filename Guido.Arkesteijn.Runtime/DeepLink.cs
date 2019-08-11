using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Guido.Arkesteijn.DeepLink.Runtime
{
    public class DeepLink
    {
        public static bool Initialized { get { return Instance != null; } }
        public static DeepLink Instance { get; private set; }

        public string ArgumentString { get { return argumentParser != null ? argumentParser.ToString() : ""; } }
        public Dictionary<string, object> Payload { get { return argumentParser.GetPayload(); } }
        private DeepLinkSettings settings;
        private IDeepLinkArgumentParser argumentParser;

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

#if UNITY_EDITOR
            Instance = new DeepLink(settings, new DeepLinkArgumentParser(settings, new string[] { "PATH", "guidoarkesteijn:product:1" }));
#else
            Instance = new DeepLink(settings, new DeepLinkArgumentParser(settings, Environment.GetCommandLineArgs()));
#endif
        }

        private DeepLink(DeepLinkSettings deepLinkSettings, IDeepLinkArgumentParser argumentParser)
        {
            this.settings = deepLinkSettings;
            this.argumentParser = argumentParser;
        }

        public void CallMethods()
        {
            SendEvent();
        }

        private void SendEvent()
        {
            if(Payload != null && Payload.Count > 0)
            {
                foreach (var item in Payload)
                {
                    var methods = MethodCache.GetMethodsWithAttribute<DeepLinkAttribute>();

                    foreach (var method in methods)
                    {
                        DeepLinkAttribute attribute = method.GetCustomAttribute<DeepLinkAttribute>();

                        if (attribute.uri == item.Key)
                        {
                            if (item.Value == null)
                            {
                                InvokeMethod(method, null);
                            }
                            else
                            {
                                InvokeMethod(method, new object[] { item.Value });
                            }
                        }
                    }
                }
            }
        }
        
        private void InvokeMethod(MethodInfo methodInfo, object[] parameters)
        {
            // null because the method needs to be static.
            methodInfo.Invoke(null, parameters);
        }
    }
}