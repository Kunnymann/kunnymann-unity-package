using UnityEngine;

namespace Kunnymann.Base
{
    /// <summary>
    /// 모듈 기본 Behaviour
    /// Awake에서 Setting을 초기화한다
    /// </summary>
    public class BaseBehaviour : MonoBehaviour, CommonBase
    {
        /// <summary>
        /// Awake에서 일반 초기화 및 Setting 초기화를 수행합니다
        /// </summary>
        protected virtual void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Setting 초기화를 수행합니다
        /// </summary>
        public virtual void Initialize()
        {
            
        }
    }
}
