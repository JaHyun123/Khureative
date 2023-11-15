using System;
using System.Collections.Generic;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Core.VivenComponents
{
    /// <summary>
    /// 오브젝트를 붙힐 때 사용되어지는 Component입니다.
    /// </summary>
    [AddComponentMenu("VivenSDK/Viven Attach Point")]
    public class SDKAttachPoint : MonoBehaviour
    {
        /// <summary>
        /// attach 가능한 prefab의 object id list
        /// </summary>
        [Tooltip("attach 가능한 prefab list")]
        [NonSerialized] public List<string> attachablePrefabs;
        
        /// <summary>
        /// attach 불가능한 prefab의 object id list
        /// </summary>
        [Tooltip("attach 불가능한 prefab list")]
        [NonSerialized] public List<string> notAttachablePrefabs;
    }
}