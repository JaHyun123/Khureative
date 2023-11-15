using System;
using System.Collections.Generic;
using TwentyOz.VivenSDK.Scripts.Core.VivenComponents.VivenFields;
using UnityEngine;
using UnityEngine.Events;

namespace TwentyOz.VivenSDK.Scripts.Core.VivenComponents
{
    /// <summary>
    /// 해당 오브젝트를 잡을 때 사용하는 Module입니다.
    /// </summary>
    [RequireComponent(typeof(SDKNetworkObject),typeof(SDKGrabbableRigidbodyView),typeof(SDKRigidbody))]
    [AddComponentMenu("VivenSDK/Viven Grabbable Module")]
    public class SDKGrabbableModule : MonoBehaviour
    {
        /// <summary>
        /// 해당 오브젝트의 Grab Type을 설정합니다.
        /// </summary>
        [Header("Grab")]
        [SerializeField] public SDKGrabType objectGrabType;
        
        /// <summary>
        /// 해당 값이 True이면 오브젝트를 잡았을 때 해당 오브젝트의 부모가 잡은 손으로 변경됩니다.
        /// </summary>
        [SerializeField] protected bool parentToHandOnGrab;
        
        /// <summary>
        /// 잡혔을 때의 포인트들
        /// </summary>
        [NonSerialized] public List<Transform> grabPoints = new List<Transform>();
        
        /// <summary>
        /// 얼마나 길게 눌러야 길게 누른 것으로 인식할지에 대한 시간
        /// </summary>
        [Header("Interaction")]
        [SerializeField] public float longClickTimeThreshold = 1.0f;
        
        [Header("Attach")] [NonSerialized] public List<SDKAttachPoint> vivenAttachPoints;

        // [NonSerialized] public List<SDKAttachPivot> vivenAttachPivots;
        
        /// <summary>
        /// place 시에, 벽에 붙을 때의 축을 결정하는 pivot
        /// </summary>
        [Header("Place Mode")]
        public Vector3 placePivot = Vector3.up;
        
        /// <summary>
        /// Grab시에 불려질 이벤트
        /// </summary>
        [Header("Events")]
        [NonSerialized]
        public UnityEvent vivenOnGrabEvent;
        
        /// <summary>
        /// 그랩 잡은상태에서 짧게 눌렀을 때 발생하는 이벤트
        /// </summary>
        [NonSerialized]
        public UnityEvent objectShortClickActionStart;

        /// <summary>
        /// 그랩 잡은상태에서 길게 누르는 것이 인식되는 순간 발생하는 이벤트
        /// </summary>
        [NonSerialized]
        public UnityEvent objectLongClickActionStart;
        
        /// <summary>
        /// <see cref="objectLongClickActionStart"/>이벤트가 불린 후 액션 버튼을 뗐을 때 발생하는 이벤트
        /// </summary>
        [NonSerialized]
        public UnityEvent objectLongClickActionEnd;
    }
} 