using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DefaultExecutionOrder(200)] // after UFE.cs
public class UFEExtension : SingletonMonoBehaviour<UFEExtension>{
    [SerializeField]
    private UFEExtensionInfo extensionInfo;
    public UFEExtensionInfo ExtensionInfo { get { return this.extensionInfo; } }
    public string extensionPath { get; private set; }

    private GameRecorder gameRecorder;

    public static readonly List<Type> aiEngines = TypeUtility.GetSubClassOf(typeof(BaseAI));

    private void Awake() {
        CheckInstance();

        extensionPath = Application.dataPath + "/UFE/Engine/Scripts/Core/Extension";

        if (extensionInfo.overrideAI) {
            TypeUtility.CallGenericStaticMethod("OverrideRandomAIwith", extensionInfo.p1AIEngine.GetSystemType(), new object[] { 1 });
            TypeUtility.CallGenericStaticMethod("OverrideRandomAIwith", extensionInfo.p2AIEngine.GetSystemType(), new object[] { 2 });
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
    /// Check if UFE Extension is working by finding a object that UFEExtension.cs is attached to in a current scene.
    /// </summary>
    /// <returns></returns>
    public static bool IsAvailable() {
        if (FindObjectOfType<UFEExtension>() != null) return true;
        else return false;
    }
}

[DefaultExecutionOrder(100)]
public partial class UFE {
    public static void OverrideRandomAIwith<T>(int player) where T : BaseAI {
        GameObject ufeManager = FindObjectOfType<UFE>().gameObject;

        if(player == 1) {
            p1RandomAI = ufeManager.AddComponent<T>();
            p1RandomAI.player = 1;
        }
        else if(player == 2) {
            p2RandomAI = ufeManager.AddComponent<T>();
            p2RandomAI.player = 2;
        }
        else {
            Debug.LogError("Player number must be either 1 or 2.");
        }
    }
}