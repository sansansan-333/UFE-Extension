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
}
