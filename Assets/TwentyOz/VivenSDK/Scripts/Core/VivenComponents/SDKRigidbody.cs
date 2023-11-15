using TwentyOz.VivenSDK.Scripts.Core.VivenComponents.VivenFields;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Core.VivenComponents
{
    [AddComponentMenu("VivenSDK/Viven Rigid body")]
    public class SDKRigidbody : MonoBehaviour
    {
        [SerializeField] public SDKPhysicsType objectPhysicsType;
        
        /// <summary>
        /// <see cref="Rigidbody"/>의 mass 입니다.
        /// </summary>
        [Header("Rigidbody Fields")]
        [SerializeField] public float originMass = 1f;
        
        /// <summary>
        /// <see cref="Rigidbody"/>의 Drag 입니다.
        /// </summary>
        [SerializeField] public float originDrag = 0f;
        
        /// <summary>
        /// <see cref="Rigidbody"/>의 AngularDrag 입니다.
        /// </summary>
        [SerializeField] public float originAngularDrag = 0.05f;
        
        /// <summary>
        /// <see cref="Rigidbody"/>의 Center Of Mass 입니다.
        /// </summary>
        [SerializeField] public Vector3 originCom;
    }
}