namespace Kunnymann.Base
{
    /// <summary>
    /// VPS engine type
    /// </summary>
    public enum TrackerType
    {
        /// <summary>
        /// 기본값
        /// </summary>
        Default = 0,
        /// <summary>
        /// 원천 기술사 VPS engine
        /// </summary>
        SourceProprietaryTech,
        /// <summary>
        /// 자사 VPS engine
        /// </summary>
        LocalTech,
        /// <summary>
        /// 시뮬레이터
        /// </summary>
        Simulator
    }
}
