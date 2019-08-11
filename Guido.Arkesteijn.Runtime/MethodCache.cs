using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

public static class MethodCache
{
    public static IEnumerable<MethodInfo> Methods { get { return methods; } }

    private static readonly MethodInfo[] methods;

    static MethodCache()
    {
        List<MethodInfo> methodList = new List<MethodInfo>();
        foreach (var item in TypeCache.Types)
        {
            methodList.AddRange(item.GetMethods());
        }

        methods = methodList.ToArray();
    }

    public static IEnumerable<MethodInfo> GetMethodsWithAttribute<T>()
    {
        return GetMethodsWithAttribute(typeof(T));
    }

    public static IEnumerable<MethodInfo> GetMethodsWithAttribute(Type type)
    {
        return methods.Where(x => x.GetCustomAttributes(type, true).Length > 0);
    }
}