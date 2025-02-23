using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Kunnymann.UI
{
    /// <summary>
    /// UI Animation Transition을 적용하는 Base Class
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UITransitionBase : MonoBehaviour
    {
        /// <summary>
        /// VisibleState에 따라 호출되는 이벤트
        /// </summary>
        public UnityEvent<VisibleState> ChangedVisibleState = new UnityEvent<VisibleState>();

        private CanvasGroup _canvasGroup;
        /// <summary>
        /// 대상 CanvasGroup
        /// </summary>
        public CanvasGroup CanvasGroup
        {
            get
            {
                _canvasGroup ??= GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }

        private VisibleState _visibleState = VisibleState.Appeared;
        /// <summary>
        /// VisibleState
        /// </summary>
        public VisibleState VisibleState
        {
            get => _visibleState;
            set
            {
                if (_visibleState == value)
                    return;
                _visibleState = value;

                if (CanvasGroup != null)
                {
                    CanvasGroup.blocksRaycasts = (_visibleState == VisibleState.Appeared || _visibleState == VisibleState.Appearing);
                }

                switch (_visibleState)
                {
                    case VisibleState.Appeared:
                        OnShow(); break;
                    case VisibleState.Appearing:
                        OnShowing(); break;
                    case VisibleState.Disappearing:
                        OnHiding(); break;
                    case VisibleState.Disappeared:
                        OnHide(); break;
                }
                ChangedVisibleState?.Invoke(_visibleState);
            }
        }

        /// <summary>
        /// 초기화 여부
        /// </summary>
        private bool initialized = false;

        /// <summary>
        /// Animation을 생략하고, 즉시 Show합니다.
        /// </summary>
        public void ShowImmediately()
        {
            Show().Forget();
        }

        /// <summary>
        /// Animation을 재생하고, Show합니다.
        /// </summary>
        /// <param name="duration">시간</param>
        /// <returns>UniTask</returns>
        public async UniTask Show(float duration = 0)
        {
            if (!initialized)
            {
                Initialize();
                initialized = true;
            }

            switch (_visibleState)
            {
                case VisibleState.Appeared:
                case VisibleState.Appearing:
                    return;
                case VisibleState.Disappearing:
                    KillAnim();
                    break;
            }

            if (duration == 0)
            {
                ShowWithoutAnim();
            }
            else
            {
                VisibleState = VisibleState.Appearing;
                await ShowAnim(duration);
            }

            VisibleState = VisibleState.Appeared;
        }

        /// <summary>
        /// Animation을 생략하고, 즉시 Hide합니다.
        /// </summary>
        public void HideImmediately()
        {
            Hide().Forget();
        }

        /// <summary>
        /// Animation을 재생하고, Hide합니다.
        /// </summary>
        /// <param name="duration">시간</param>
        /// <returns>UniTask</returns>
        public async UniTask Hide(float duration = 0)
        {
            if (!initialized)
            {
                Initialize();
                initialized = true;
            }

            switch (_visibleState)
            {
                case VisibleState.Disappeared:
                case VisibleState.Disappearing:
                    return;
                case VisibleState.Appearing:
                    KillAnim();
                    break;
            }

            if (duration == 0)
            {
                HideWithoutAnim();
            }
            else
            {
                VisibleState = VisibleState.Disappearing;
                await HideAnim(duration);
            }

            VisibleState = VisibleState.Disappeared;
        }

        /// <summary>
        /// 애니메이션 실행 직전, 초기화를 진행하는 함수입니다.
        /// </summary>
        protected virtual void Initialize() { }

        /// <summary>
        /// 지정된 시간동안 Show 애니메이션을 재생하는 내부 함수입니다.
        /// </summary>
        /// <param name="duration">시간</param>
        /// <returns>UniTask</returns>
        protected abstract UniTask ShowAnim(float duration);

        /// <summary>
        /// 지정된 시간동안 Hide 애니메이션을 재생하는 내부 함수입니다.
        /// </summary>
        /// <param name="duration">시간</param>
        /// <returns>UniTask</returns>
        protected abstract UniTask HideAnim(float duration);

        /// <summary>
        /// 즉시 Show를 실행하는 내부 함수입니다.
        /// </summary>
        protected abstract void ShowWithoutAnim();

        /// <summary>
        /// 즉시 Hide를 실행하는 내부 함수입니다.
        /// </summary>
        protected abstract void HideWithoutAnim();

        /// <summary>
        /// 재생 중인 Animation을 즉시 종료하는 내부 함수입니다.
        /// </summary>
        protected abstract void KillAnim();

        /// <summary>
        /// VisibleState가 Appeared로 변경되었을 때 호출되는 함수입니다.
        /// </summary>
        protected virtual void OnShow() { }

        /// <summary>
        /// VisibleState가 Appearing로 변경되었을 때 호출되는 함수입니다.
        /// </summary>
        protected virtual void OnShowing() { }

        /// <summary>
        /// VisibleState가 Disappeared로 변경되었을 때 호출되는 함수입니다.
        /// </summary>
        protected virtual void OnHide() { }

        /// <summary>
        /// VisibleState가 Disappearing로 변경되었을 때 호출되는 함수입니다.
        /// </summary>
        protected virtual void OnHiding() { }
    }

}
