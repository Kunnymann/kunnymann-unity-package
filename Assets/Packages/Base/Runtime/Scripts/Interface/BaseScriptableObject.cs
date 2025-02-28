using UnityEngine;

namespace Kunnymann.Base
{
    /// <summary>
    /// 모듈 기본 ScriptableObject
    /// 직접 Initialize를 호출하여 초기화를 한다
    /// </summary>
    public class BaseScriptableObject : ScriptableObject, CommonBase
    {
        /// <summary>
        /// 일반 초기화 및 Setting 초기화를 수행합니다
        /// </summary>
        public virtual void Initialize()
        {
            
        }
    }
}