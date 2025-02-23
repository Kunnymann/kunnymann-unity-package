namespace Kunnymann.Base
{
    /// <summary>
    /// 모듈의 상태
    /// </summary>
    public enum ModuleState
    {
        /// <summary>
        /// 생성
        /// </summary>
        Created,
        /// <summary>
        /// 비활성화
        /// </summary>
        Inactive,
        /// <summary>
        /// 활성화
        /// </summary>
        Active,
        /// <summary>
        /// 제거
        /// </summary>
        Terminated
    }

    /// <summary>
    /// 모듈의 상태 전이를 위한 명령어
    /// </summary>
    public enum TransitionCommand
    {
        /// <summary>
        /// 초기화 명령
        /// </summary>
        Initialize,
        /// <summary>
        /// 명령 없음
        /// </summary>
        None,
        /// <summary>
        /// 제거 명령
        /// </summary>
        Terminate
    }
}