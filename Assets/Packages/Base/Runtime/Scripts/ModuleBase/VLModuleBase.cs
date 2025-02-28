using Kunnymann.Base.Data;
using UnityEngine;

namespace Kunnymann.Base.DI
{
    /// <summary>
    /// VPS Module의 DI 객체
    /// </summary>
    public abstract class VLModuleBase : ModuleBase
    {
        #pragma warning disable 67
        /// <summary>
        /// VL 상태 변경에 대한 추상 이벤트
        /// </summary>
        public abstract event EventHandler<VLState> StateChanged;
        /// <summary>
        /// VL 위치 변경에 대한 추상 이벤트
        /// </summary>
        public abstract event EventHandler<Pose> PoseUpdated;
        /// <summary>
        /// VL 성공 위치 변경에 대한 추상 이벤트
        /// </summary>
        public abstract event EventHandler<Pose> SuccessPoseUpdated;
        /// <summary>
        /// VL Layer 정보 변경에 대한 추상 이벤트
        /// </summary>
        public abstract event EventHandler<string> LayerInfoChanged;
        /// <summary>
        /// VL GPS 좌표 변경에 대한 추상 이벤트
        /// </summary>
        public abstract event EventHandler<float, float> GeoCoordUpdated;
        #pragma warning restore 67

        /// <summary>
        /// Tracker Type을 반환
        /// </summary>
        public abstract TrackerType TrackerType { get; }
        /// <summary>
        /// VL State를 반환
        /// </summary>
        public abstract VLState State { get; }
        /// <summary>
        /// Current Layer를 반환
        /// </summary>
        public abstract string CurrentLayer { get; }
        /// <summary>
        /// 초기화 여부를 반환
        /// </summary>
        public abstract bool IsInitialized { get; }
        /// <summary>
        /// VLModule의 Run 여부를 반환
        /// </summary>
        public abstract bool IsRun { get; }
        /// <summary>
        /// 현재 Tracker의 Pose를 반환
        /// </summary>
        public abstract Pose CurrentPose { get; }

        /// <summary>
        /// VPS를 실행합니다
        /// </summary>
        public abstract void Run();
        /// <summary>
        /// VPS를 초기화합니다
        /// </summary>
        /// <param name="type">Target VPS</param>
        /// <returns>성공 여부</returns>
        public abstract bool Init();
        /// <summary>
        /// VPS를 중지합니다
        /// </summary>
        public abstract void Stop();
        /// <summary>
        /// VPS를 재시작합니다
        /// </summary>
        public abstract void Restart();
        /// <summary>
        /// 위경도 값을 강제로 주입합니다
        /// </summary>
        /// <param name="longitude">경도</param>
        /// <param name="latitude">위도</param>
        public abstract void ForceGeoCoord(float longitude, float latitude);
        /// <summary>
        /// VPS로 Texture를 전송합니다
        /// </summary>
        /// <param name="texture"></param>
        public abstract void SendTexture(Texture texture);
    }
}
