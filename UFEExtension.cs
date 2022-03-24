using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UFE3D;

[DefaultExecutionOrder(100)] 
public partial class UFE {
    public static void OverrideRandomAIwith<T>() where T : BaseAI{
        GameObject ufeManager = FindObjectOfType<UFE>().gameObject;

        p1RandomAI = ufeManager.AddComponent<T>();
        p1RandomAI.player = 1;
        p2RandomAI = ufeManager.AddComponent<T>();
        p2RandomAI.player = 2;
    }
}

[DefaultExecutionOrder(200)] // after UFE.cs
public class UFEExtension : SingletonMonoBehaviour<UFEExtension>{
    [SerializeField]
    private UFEExtensionInfo extensionInfo;
    public string extensionPath { get; private set; }

    private GameRecorder gameRecorder;

    public static readonly List<Type> aiEngines = TypeUtility.GetSubClassOf(typeof(BaseAI));

    private void Awake() {
        CheckInstance();

        extensionPath = Application.dataPath + "/UFE/Engine/Scripts/Core/Extension";

        if (extensionInfo.overrideAI) {
            // call UFE.OverrideRandomAIWith<T> with selected AI type
            var mi = typeof(UFE).GetMethod("OverrideRandomAIwith");
            var genericMi = mi.MakeGenericMethod(extensionInfo.aiEngine.GetSystemType());
            genericMi.Invoke(null, null);
        }

        if (extensionInfo.recordGame) {
            gameRecorder = gameObject.AddComponent<GameRecorder>();
            gameRecorder.Initialize(extensionInfo.savePath);
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ForceToUseRandomAI() {
        if(UFEExtension.IsAvailable() && FindObjectOfType<UFEExtension>().extensionInfo.overrideAI) {
            UFE ufe = FindObjectOfType<UFE>();
            ufe.UFE_Config.aiOptions.engine = AIEngine.RandomAI;
        }
    }

    /// <summary>
    /// Check if UFE Extension is working by finding a object that UFEExtension.cs is attached to in current scene.
    /// </summary>
    /// <returns></returns>
    public static bool IsAvailable() {
        if (FindObjectOfType<UFEExtension>() != null) return true;
        else return false;
    }
}
