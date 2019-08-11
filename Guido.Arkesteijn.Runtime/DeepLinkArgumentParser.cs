using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guido.Arkesteijn.DeepLink.Runtime
{
    public interface IDeepLinkArgumentParser
    {
        bool GetArgumentValue(string key, out object value);
        Dictionary<string, object> GetPayload();
    }

    public class DeepLinkArgumentParser : IDeepLinkArgumentParser
    {
        private string[] arguments;
        private Dictionary<string, object> payload = new Dictionary<string, object>();

        public DeepLinkArgumentParser(DeepLinkSettings deepLinkSettings, string[] arguments)
        {
            this.arguments = arguments;

            //Skip 0 because this is the excutablePath on Windows
            for (int i = 1; i < arguments.Length; i++)
            {
                string key = null;
                object value = null;
                string[] values = arguments[i].Split(':');

                if(values.Length > 0 && values[0] == deepLinkSettings.Domain)
                {
                    if (values.Length > 1)
                    {
                        key = values[1];
                    }
                    if (values.Length > 2)
                    {
                        value = GetObjectValue(values[2]);
                    }
                    payload.Add(key, value);
                }
            }
        }

        public object GetObjectValue(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                return null;
            }
            if(int.TryParse(value, out int i))
            {
                return i;
            }
            else if(float.TryParse(value, out float f))
            {
                return f;
            }
            
            return value;
        }

        public bool GetArgumentValue(string key, out object value)
        {
            return payload.TryGetValue(key, out value);
        }

        public Dictionary<string, object> GetPayload()
        {
            return payload;
        }

        public override string ToString()
        {
            string value = "";
            foreach (var item in arguments)
            {
                value += item + ",";
            }
            return value;
        }
    }
}