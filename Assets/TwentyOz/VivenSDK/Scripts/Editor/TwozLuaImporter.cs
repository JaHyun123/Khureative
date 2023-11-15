using TwentyOz.VivenSDK.Scripts.Core.Lua;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Editor
{
    [ScriptedImporter(1, "lua")]
    public class TwozLuaImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var createdAsset = ScriptableObject.CreateInstance<VivenScript>(); 
            createdAsset.scriptString = System.IO.File.ReadAllText(ctx.assetPath);
            ctx.AddObjectToAsset(ctx.assetPath, createdAsset);
            ctx.SetMainObject(createdAsset);
        }
    }
}
