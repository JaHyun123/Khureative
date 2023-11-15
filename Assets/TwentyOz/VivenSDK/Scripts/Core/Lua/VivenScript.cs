#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Core.Lua
{
    public class VivenScript : ScriptableObject
    {
#if UNITY_EDITOR
        [MenuItem("Assets/Create/VivenScriptable")]
        public static void CreateAsset()
        {
            // create file in Assets folder
            var luaFile = new System.IO.StreamWriter(EditorUtility.SaveFilePanel("Create Lua File", "Assets", "NewLuaFile", "lua"));
            luaFile.WriteLine("");
            luaFile.Close();

            // Refresh AssetDatabase
            AssetDatabase.Refresh();
            // Refresh Proejct
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
        }
#endif

        public string scriptString;
    }
}