using TwentyOz.VivenSDK.Scripts.Core.Lua;
using UnityEditor;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Editor
{
    public class VivenLuaBehaviourCreator
    {
        [MenuItem("GameObject/Create Other/VivenLuaBehaviour")]
        public static void MakeLuaBehaviour()
        {
            var go = new GameObject("VivenLuaBehaviour");
            go.AddComponent<VivenLuaBehaviour>();
            Selection.activeGameObject = go;
            
        } 
    }
}
