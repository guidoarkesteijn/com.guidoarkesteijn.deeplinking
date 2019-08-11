using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Caches all types in the current assembly to allow for fast lookup of types without minimal memory allocation
/// </summary>
public static class TypeCache
{
    public static IEnumerable<Type> Types { get { return types; } }

    private static readonly Type[] types;
    private static readonly Dictionary<string, Type> typeLookup;

    static TypeCache()
    {
#if UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE
        // Only get assemblies which are made from custom code.
        List<Type> typeList = new List<Type>();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < assemblies.Length; i++)
        {
            if (assemblies[i].GetName().Name.Contains("Assembly"))
            {
                typeList.AddRange(assemblies[i].GetTypes());
            }
        }
        types = typeList.ToArray();
#else
		types = typeof(TypeCache).GetTypeInfo().Assembly.GetTypes();
#endif
        typeLookup = new Dictionary<string, Type>();
        for (int i = 0; i < types.Length; i++)
        {
            typeLookup[types[i].FullName] = types[i];
        }
    }

    /// <summary>
    /// Get type by name 
    /// </summary>
    public static Type Get(string name)
    {
        Type result;
        typeLookup.TryGetValue(name, out result);
        return result;
    }

    /// <summary>
    /// Filter the assembly types to the ones that can be assigned to the input type.
    /// </summary>
    /// <param name="type">Type to check assingablity to</param>
    /// <returns>Collection of types assingable to the input type </returns>
    public static IEnumerable<Type> GetTypesAssignableTo<T>()
    {
        return GetTypesAssignableTo(typeof(T));
    }

    /// <summary>
    /// Filter the assembly types to the ones that can be assigned to the input type.
    /// </summary>
    /// <param name="type">Type to check assingablity to</param>
    /// <returns>Collection of types assingable to the input type </returns>
    public static IEnumerable<Type> GetTypesAssignableTo(Type type)
    {
        return types.Where(t => t != type && type.IsAssignableFrom(t));
    }

    public static IEnumerable<Type> GetTypesWithAttribute<T>()
    {
        return GetTypesWithAttribute(typeof(T));
    }

    public static IEnumerable<Type> GetTypesWithAttribute(Type type)
    {
        return types.Where(t => t.GetCustomAttributes(type, true).Length > 0);
    }
}