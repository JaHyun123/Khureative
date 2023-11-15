using System;
using TwentyOz.VivenSDK.Scripts.Core.Lua;
using UnityEditor;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Editor
{
    [CustomEditor(typeof(VivenLuaBehaviour))]
    public class VivenLuaBehaviourCustomInspector : UnityEditor.Editor
    {
        private SerializedProperty _luaScript;
        private SerializedProperty _injection;
        private SerializedProperty _shouldSync;
        private SerializedProperty _isGrabbable;

        private void OnEnable()
        {
            _luaScript = serializedObject.FindProperty("luaScript");
            _injection = serializedObject.FindProperty("injection");
            _shouldSync = serializedObject.FindProperty("shouldSync");
            _isGrabbable = serializedObject.FindProperty("isGrabbable");
        }

        public override void OnInspectorGUI()
        {
            var titleGuiLayout = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize  = 20,
                wordWrap  = true
            };
            GUILayout.Label("VIVEN 기본 스크립트", titleGuiLayout);
            // 자동 줄바꿈
            var subtitleGuiLayout = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize  = 15,
                normal    = { textColor = Color.gray },
                wordWrap  = true
            };
            GUILayout.Label("VivenLuaBehaviour는 VivenSDK에서 사용하는 기본 스크립트입니다.", subtitleGuiLayout);
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_luaScript, new GUIContent(string.Empty));


            // LuaScriptable을 만들고 Property에 할당
            if (GUILayout.Button("VIVEN Script 만들기"))
            {
                var fileName = EditorUtility.SaveFilePanel("Create Lua File", "Assets", "NewLuaFile", "lua");
                var luaFile  = new System.IO.StreamWriter(fileName);
                luaFile.WriteLine("");
                luaFile.Close();
                AssetDatabase.Refresh();
                EditorApplication.ExecuteMenuItem("Assets/Refresh");
                var assetPath       = fileName.Substring(fileName.IndexOf("Assets", StringComparison.Ordinal));
                var vivenScriptable = AssetDatabase.LoadAssetAtPath<VivenScript>(assetPath);
                _luaScript.objectReferenceValue = vivenScriptable;
            }

            GUILayout.EndHorizontal();
            
            // EditorGUILayout.PropertyField(_shouldSync, new GUIContent("네트워크 동기화 필요 여부", "이 필드가 true일 경우 네트워크 동기화가 이루어집니다."));
            // EditorGUILayout.PropertyField(_isGrabbable, new GUIContent("잡을 수 있는 물체 여부", "이 필드가 true일 경우 물체를 잡을 수 있습니다."));

            EditorGUILayout.PropertyField(_injection);
            serializedObject.ApplyModifiedProperties();
        }
    }
}