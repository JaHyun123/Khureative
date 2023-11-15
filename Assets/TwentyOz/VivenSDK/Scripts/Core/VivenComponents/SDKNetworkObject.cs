using System;
using TwentyOz.VivenSDK.Scripts.Core.VivenComponents.VivenFields;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Core.VivenComponents
{
    /// <summary>
    /// Object를 동기화를 하거나, 상호작용이 가능할 수 있도록 사용하는 Component입니다.
    /// </summary>
    ///
    [AddComponentMenu("VivenSDK/Viven Network Object")]
    public class SDKNetworkObject : MonoBehaviour
    {
        /// <summary>
        /// 해당 오브젝트의 이름입니다.
        /// </summary>
        public string displayName;
        
        /// <summary>
        /// Networking할 때 사용되는 Object의 ID입니다.
        /// </summary>
        public string objectId;
        /// <summary>
        /// 해당 Object의 Content Type입니다. 
        /// </summary>
        [NonSerialized] public SDKContentType contentType = SDKContentType.Prepared;

        /// <summary>
        /// Object의 동기화 방식입니다.
        /// </summary>
        public SDKSyncType objectSyncType  = SDKSyncType.Continuous;
        

        private void Reset()
        {
            objectId = Guid.NewGuid().ToString();
        }
        
        
    }
}
