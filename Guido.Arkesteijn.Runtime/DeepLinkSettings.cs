using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guido.Arkesteijn.DeepLink.Runtime
{
    [CreateAssetMenu(menuName = "Guido Arkesteijn/Deep Linking", fileName = nameof(DeepLinkSettings))]
    public class DeepLinkSettings : ScriptableObject
    {
        public static DeepLinkSettings DefaultSettings
        {
            get
            {
                DeepLinkSettings defaultSettings = CreateInstance(typeof(DeepLinkSettings)) as DeepLinkSettings;
                return defaultSettings;
            }
        }

        public string Domain { get { return domain; } }
        [SerializeField] private string domain;
    }
}