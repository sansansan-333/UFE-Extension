using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class TypeUtility
{
    /// <summary>
    /// Returns a list of subclasses
    /// </summary>
    /// <param name="t">parent type</param>
    /// <returns></returns>
    public static List<Type> GetSubClassOf(Type t) {
        var result = new List<Type>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
            foreach (var type in assembly.GetTypes()) {
                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(t)) {
                    result.Add(type);
                }
            }
        }

        return result;
    }

    public static System.Reflection.MethodInfo GetGenericMethodInfo(string methodName, Type t) {
        var mi = typeof(UFE).GetMethod(methodName);
        return mi.MakeGenericMethod(t);
    }

    public static object CallGenericMethod(string methodName, Type t, object instance, object[] parameters) {
        return GetGenericMethodInfo(methodName, t).Invoke(instance, parameters);
    }

    public static object CallGenericStaticMethod(string methodName, Type t, object[] parameters) {
        return CallGenericMethod(methodName, t, null, parameters);
    }
}
