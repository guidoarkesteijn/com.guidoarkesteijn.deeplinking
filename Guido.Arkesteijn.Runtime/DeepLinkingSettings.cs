using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guido.Arkesteijn.Runtime
{
    [CreateAssetMenu(menuName = "Guido Arkesteijn/Deep Linking", fileName = nameof(DeepLinkingSettings))]
    public class DeepLinkingSettings : ScriptableObject
    {
        public static DeepLinkingSettings DefaultSettings
        {
            get
            {
                DeepLinkingSettings defaultSettings = CreateInstance(typeof(DeepLinkingSettings)) as DeepLinkingSettings;
                return defaultSettings;
            }
        }

        public string Domain { get { return domain; } }
        [SerializeField] private string domain;
    }
}