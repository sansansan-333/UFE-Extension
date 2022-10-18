using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System;

/// <summary>
/// Custom window for added features in UFE
/// </summary>
public class UFEExtensionWindow : EditorWindow 
{
    private UFEExtensionInfo extensionInfo;
    private Vector2 scrollPos;

    // AI
    private bool aiSettings;
    private int p1AIEngineIndex;
    private int p2AIEngineIndex;

    // Game recording
    private bool recordSettings;

    [MenuItem("Window/U.F.E./Extension")]
    public static void Init() {
        var window = GetWindow<UFEExtensionWindow>(false, "Extension", true);
        window.Show();
        window.Populate();
    }

    void OnSelectionChange() {
        Populate();
        Repaint();
    }

    private void Populate() {
        titleContent = new GUIContent("Extension");

        // get selected UFE Extensioin file
        UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UFEExtensionInfo), SelectionMode.Assets);
        if (selection.Length > 0) {
            if (selection[0] == null) return;
            extensionInfo = (UFEExtensionInfo)selection[0];
        }
    }

    void OnGUI() {
        if(extensionInfo == null) {
            EditorGUILayout.BeginHorizontal("GroupBox");
            EditorGUILayout.LabelField("Select a UFE Extension file\nor create a new one.", GUILayout.Height(50));
            EditorGUILayout.EndHorizontal();
            return;
        }

        if (!UFEExtension.IsAvailable()) {
            EditorGUILayout.BeginHorizontal("GroupBox");
            EditorGUILayout.LabelField("Attach UFEExtension.cs to a game object in current scene.", GUILayout.Height(50));
            EditorGUILayout.EndHorizontal();
            return;
        }

        EditorGUI.BeginChangeCheck();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        {
            EditorGUIUtility.labelWidth = 150;

            // AI setttings
            EditorGUILayout.BeginVertical("GroupBox");
            {
                aiSettings = EditorGUILayout.Foldout(aiSettings, "AI Settings");

                if (aiSettings) {
                    // override AI?
                    EditorGUILayout.BeginHorizontal();
                    extensionInfo.overrideAI = EditorGUILayout.Toggle("Override AI", extensionInfo.overrideAI);
                    EditorGUILayout.EndHorizontal();

                    if (extensionInfo.overrideAI) {
                        // AI engine
                        if (UFEExtension.aiEngines.Count != 0) {
                            EditorGUILayout.LabelField("AI engine");

                            EditorGUI.indentLevel++;
                            {
                                p1AIEngineIndex = UFEExtension.aiEngines.FindIndex(ai => ai == extensionInfo.p1AIEngine.GetSystemType());
                                p2AIEngineIndex = UFEExtension.aiEngines.FindIndex(ai => ai == extensionInfo.p2AIEngine.GetSystemType());
                                var aiEngineOptions = UFEExtension.aiEngines.Select(elem => elem.ToString()).ToArray();

                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("Player 1");
                                    p1AIEngineIndex = EditorGUILayout.Popup(p1AIEngineIndex, aiEngineOptions);
                                }
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("Player 2");
                                    p2AIEngineIndex = EditorGUILayout.Popup(p2AIEngineIndex, aiEngineOptions);
                                }
                                EditorGUILayout.EndHorizontal();

                                extensionInfo.p1AIEngine = new SerializableSystemType(UFEExtension.aiEngines[p1AIEngineIndex]);
                                extensionInfo.p2AIEngine = new SerializableSystemType(UFEExtension.aiEngines[p2AIEngineIndex]);
                            }
                            EditorGUI.indentLevel--;
                        }
                    }
                }
            }
            EditorGUILayout.EndVertical();

            // game recording
            EditorGUILayout.BeginVertical("GroupBox");
            {
                recordSettings = EditorGUILayout.Foldout(recordSettings, "Game Recording");

                if (recordSettings) {
                    // record game?
                    EditorGUILayout.BeginHorizontal();
                    extensionInfo.recordGame = EditorGUILayout.Toggle("Record games", extensionInfo.recordGame);
                    EditorGUILayout.EndHorizontal();

                    if (extensionInfo.recordGame) {
                        // warning
                        if (string.IsNullOrEmpty(extensionInfo.savePath) && extensionInfo.recordGame) {
                            EditorGUILayout.HelpBox("Select a folder to save Game Recordings.", MessageType.Warning);
                        }

                        // folder to save recordings
                        EditorGUILayout.BeginVertical();
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Folder");

                                if (GUILayout.Button("Select folder")) {
                                    string path = EditorUtility.OpenFolderPanel("Folder to save Game Recordings", "Assets", "");
                                    if (!string.IsNullOrEmpty(path)) {
                                        extensionInfo.savePath = path;
                                    }
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                            EditorGUI.indentLevel++;
                            {
                                extensionInfo.savePath = EditorGUILayout.TextField(extensionInfo.savePath);
                            }
                            EditorGUI.indentLevel--;
                        }
                        EditorGUILayout.EndVertical();

                        // description
                        EditorGUILayout.BeginVertical();
                        {
                            EditorGUILayout.LabelField("Description");
                            EditorGUI.indentLevel++;
                            {
                                extensionInfo.description = EditorGUILayout.TextArea(extensionInfo.description);
                            }
                            EditorGUI.indentLevel--;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();

        // save UFE extension file
        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(extensionInfo);
            AssetDatabase.SaveAssets();
        }
    }
}