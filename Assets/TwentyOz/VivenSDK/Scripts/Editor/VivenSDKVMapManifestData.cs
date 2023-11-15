using System;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Editor
{
    [Serializable]
    public struct VivenSDKVMapManifestData
    {
        [SerializeField] public string cttDataType; //해당 Map의 DataType SDK에서는 Addressable임.
        [SerializeField] public string binval; //해당 Scene의 uuid
        [SerializeField] public string cttId;
    }
}