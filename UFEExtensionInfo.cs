using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class UFEExtensionInfo : ScriptableObject {
    // AI
    public bool overrideAI;
    public SerializableSystemType p1AIEngine;
    public SerializableSystemType p2AIEngine;

    // Game recording
    public bool recordGame;
    public string savePath;
    public string description;
}
