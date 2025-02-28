namespace Kunnymann.Base
{
    /// <summary>
    /// 모듈 기본 오브젝트
    /// 생성자에서 초기화를 한다
    /// </summary>
    public class BaseObject : CommonBase
    {
        /// <summary>
        /// 생성자
        /// </summary>
        public BaseObject() 
        {
            Initialize();
        }

        /// <summary>
        /// 일반 초기화 및 Setting 초기화를 수행합니다
        /// </summary>
        public virtual void Initialize()
        {

        }
    }
}