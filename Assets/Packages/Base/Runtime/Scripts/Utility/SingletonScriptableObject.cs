using UnityEngine;
using System;
using Kunnymann.Base.Debugger;

namespace Kunnymann.Utility
{
    /// <summary>
    /// Scriptable object 싱글톤 추상 클래스
    /// </summary>
    /// <typeparam name="T">Scriptable object 클래스</typeparam>
    public abstract class SingletonScripatableObject<T> : ScriptableObject where T : SingletonScripatableObject<T>
    {
        /// <summary>
        /// lazy 싱글턴 객체
        /// </summary>
        private static T _Instance;

        /// <summary>
        /// 싱글턴 객체
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    T[] asset = Resources.LoadAll<T>(string.Empty);
                    if (asset == null || asset.Length < 1)
                    {
                        ErrorListener.Check(new Exception(), $"{nameof(T)}의 ScriptableObject를 Resource에서 찾을 수 없습니다.", needthrowerror: true);
                    }
                    else if (asset.Length > 1)
                    {
                        ErrorListener.Check(new Exception(), $"중복된 ScriptableObject ({nameof(T)})가 있습니다.", needthrowerror: true);
                    }

                    _Instance = asset[0];
                }

                return _Instance;
            }
        }
    }
}