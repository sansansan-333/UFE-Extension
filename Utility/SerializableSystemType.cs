using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SerializableSystemType
{
    private string assemblyQualifiedName;

    public SerializableSystemType(Type t) {
        assemblyQualifiedName = t.AssemblyQualifiedName;
    }

    public Type GetSystemType() {
        return Type.GetType(assemblyQualifiedName);
    }
}
