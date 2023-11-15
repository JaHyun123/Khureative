using System;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Core.Manager
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    if (FindObjectsOfType<T>().Length > 1)
                        throw new Exception($"There is other {typeof(T).Name}. Check the Hierarchy in a Scene");

                    _instance = FindObjectOfType<T>();
                }

                if (!_instance)
                {
                    var go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                }

                return _instance;
            }
        }

        protected virtual void OnDisable()
        {
            _instance = null;
        }
    }
}