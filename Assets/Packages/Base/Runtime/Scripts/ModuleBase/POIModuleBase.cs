using Kunnymann.Base.Data;

namespace Kunnymann.Base.DI
{
    /// <summary>
    /// POI DI 객체
    /// </summary>
    public class POIModuleBase : ModuleBase
    {
        /// <summary>
        /// 초기화 여부
        /// </summary>
        public virtual bool Initialized { get; }
        /// <summary>
        /// POI를 추가합니다
        /// </summary>
        /// <param name="poi">POI 객체</param>
        public virtual void AddPOI(POIFeatureData poi) { }
    }
}