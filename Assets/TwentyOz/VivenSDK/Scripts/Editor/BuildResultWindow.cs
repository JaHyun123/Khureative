using UnityEditor;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Editor
{
    public class BuildResultWindow : EditorWindow
    {
        public static void ShowWindow()
        {
            var window = GetWindow<BuildResultWindow>();
            window.titleContent = new GUIContent("Finished");
            
            window.Show();
        }

        private void OnGUI()
        {
            CreateGUI();
        }

        private void CreateGUI()
        {
            GUILayout.TextArea("빌드가 완료되었습니다.");
            if(GUILayout.Button("Close"))
                Close();
            
            // GUILayout.Label("Build Result", EditorStyles.boldLabel);
            // EditorGUILayout.TextField("CTT ID", _cttId);
            // EditorGUILayout.TextField("CTT BIN ID", _cttBinId);
        }
    }
}