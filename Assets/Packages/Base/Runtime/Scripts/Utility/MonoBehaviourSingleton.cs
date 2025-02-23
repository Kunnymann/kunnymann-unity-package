using System;
using UnityEngine;

namespace Kunnymann.Base.Utility
{
    /// <summary>
    /// MonoBehaviour 싱글톤 추상 클래스
    /// </summary>
    /// <typeparam name="T">MonoBehaviour 클래스</typeparam>
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// 싱글턴 객체 이름
        /// </summary>
        private const string ROOT_NAME = "< SingletonRoot >";
        
        /// <summary>
        /// lazy 싱글턴 객체
        /// </summary>
        private static readonly Lazy<T> _Instance = new Lazy<T>(() =>
        {
            T instance = FindObjectOfType(typeof(T)) as T;
            
            if (instance == null)
            {
                var singletonRoot = GameObject.Find(ROOT_NAME);
                if (singletonRoot == null)
                {
                    singletonRoot = new GameObject(ROOT_NAME);
                    DontDestroyOnLoad(singletonRoot);
                }

                instance = singletonRoot.AddComponent(typeof(T)) as T;
            }
                
            return instance;
        });

        /// <summary>
        /// 싱글턴 객체
        /// </summary>
        public static T Instance => _Instance.Value;
    }
}