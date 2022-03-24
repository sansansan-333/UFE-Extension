using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

/// <summary>
/// Custom window for added features in UFE
/// </summary>
public class UFEExtensionWindow : EditorWindow 
{
    private UFEExtensionInfo extensionInfo;
    private Vector2 scrollPos;

    private bool aiSettings;
    private int aiEngineindex;

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
                            var aiEngineOptions = UFEExtension.aiEngines.Select(elem => elem.ToString()).ToArray();
                            aiEngineindex = EditorGUILayout.Popup(aiEngineindex, aiEngineOptions);
                            extensionInfo.aiEngine = new SerializableSystemType(UFEExtension.aiEngines[aiEngineindex]);
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
                            extensionInfo.savePath = EditorGUILayout.TextField(extensionInfo.savePath);
                            EditorGUI.indentLevel--;
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();

        // save
        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(extensionInfo);
            AssetDatabase.SaveAssets();
        }
    }
}