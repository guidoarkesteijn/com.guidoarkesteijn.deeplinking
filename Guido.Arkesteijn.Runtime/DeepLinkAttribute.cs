using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guido.Arkesteijn.DeepLink.Runtime
{
    public class DeepLinkAttribute : Attribute
    {
        public string uri { get; set; }
    }
}
