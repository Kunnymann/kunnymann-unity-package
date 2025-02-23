namespace Kunnymann.Base
{
    /// <summary>
    /// VPS engine state
    /// </summary>
    public enum VLState
    {
        /// <summary>
        /// 초기화 대기
        /// </summary>
        None,
        /// <summary>
        /// 초기화 완료
        /// </summary>
        Initial,
        /// <summary>
        /// VPS로부터 응답이 수신된 상태
        /// </summary>
        Received,
        /// <summary>
        /// 인식 실패
        /// </summary>
        Fail,
        /// <summary>
        /// 인식 성공
        /// </summary>
        Success,
        /// <summary>
        /// 서비스 미지원
        /// </summary>
        OutofService,
    }
}
