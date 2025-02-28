using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kunnymann.UI.Popup
{
    /// <summary>
    /// Popup GUI의 최소 단위인 MonoBeheviour
    /// </summary>
    public class PopupUnit : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] protected CanvasGroup _contentCanvasGroup;
        [SerializeField] protected CanvasGroup _backgroundCanvasGroup;
        [SerializeField] protected float _showAnimDuration = 1f;
        [SerializeField] protected float _hideAnimDuration = 1f;

        [Header("Data Components")]
        [SerializeField] private List<Text> _textFields;
        [SerializeField] private List<Button> _buttons;

        /// <summary>
        /// 동적으로 Text 내용을 할당할 TextField List입니다.
        /// </summary>
        public List<Text> TextFields => _textFields;
        /// <summary>
        /// 동적으로 Listener를 할당할 Button List입니다.
        /// </summary>
        public List<Button> Buttons => _buttons;
        /// <summary>
        /// Popup의 VisibleState.
        /// </summary>
        public VisibleState VisibleState { get; private set; } = VisibleState.Disappeared;

        private ViewUnit _view;
        /// <summary>
        /// 현재 Popup의 UIView.
        /// </summary>
        public ViewUnit View
        {
            get => _view;
            set
            {
                _view?.ChangedVisibleState.RemoveListener(OnViewVisibleStateChanged);
                _view = value;
                _view.ChangedVisibleState.AddListener(OnViewVisibleStateChanged);
            }
        }

        private static PopupUnit _instance;
        /// <summary>
        /// 현재 팝업된 인스턴스를 반환합니다.
        /// </summary>
        public static PopupUnit Instance => _instance;

        /// <summary>
        /// UIPopup을 Container로부터 얻어와, Instantiate합니다.
        /// </summary>
        /// <param name="name">UIPopup prefab 이름</param>
        /// <returns>UIPopup</returns>
        public static PopupUnit GetUIPopup(string name)
        {
            var uiPopup = PopupContainer.Instance.GetUIPopup(name);
            if (uiPopup == null)
            {
                Debug.LogError($"{name}의 UIPopup이 Container 목록에 존재하지 않습니다.");
                return null;
            }

            var contentUITransition = uiPopup._contentCanvasGroup?.GetComponent<UITransitionBase>();
            if (contentUITransition == null)
            {
                contentUITransition = uiPopup._contentCanvasGroup.gameObject.AddComponent<UIFadeInOut>();
            }
            contentUITransition.HideImmediately();

            var backgroundUITransition = uiPopup._backgroundCanvasGroup.GetComponent<UITransitionBase>();
            if (backgroundUITransition == null)
            {
                backgroundUITransition = uiPopup._backgroundCanvasGroup.gameObject.AddComponent<UIFadeInOut>();
            }
            backgroundUITransition.HideImmediately();

            uiPopup.name = name;
            return uiPopup;
        }

        /// <summary>
        /// UIPopup을 즉시 Show합니다.
        /// </summary>
        public void ShowImmediately()
        {
            Show(0).Forget();
        }

        /// <summary>
        /// UIPopup을 애니메이션에 따라 Show합니다.
        /// </summary>
        /// <returns>UniTask</returns>
        public async virtual UniTask Show()
        {
            _instance = this;
            await _instance.ShowInternal(_showAnimDuration);
        }

        /// <summary>
        /// UIPopup을 애니메이션에 따라 duration 동안, Show합니다.
        /// </summary>
        /// <param name="duration">애니메이션 시간</param>
        /// <returns>UniTask</returns>
        public async virtual UniTask Show(float duration)
        {
            _instance = this;
            await _instance.ShowInternal(duration);
        }

        /// <summary>
        /// UIPopup을 즉시 Hide합니다.
        /// </summary>
        public void HideImmediately()
        {
            Hide(0).Forget();
        }

        /// <summary>
        /// UIPopup을 애니메이션에 따라 Hide합니다.
        /// </summary>
        /// <returns>UniTask</returns>
        public async virtual UniTask Hide()
        {
            if (_instance != null)
            {
                await _instance.HideInternal(_hideAnimDuration);
                Destroy();
            }
        }

        /// <summary>
        /// UIPopup을 애니메이션에 따라 duration 동안, Hide합니다.
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public async virtual UniTask Hide(float duration)
        {
            if (_instance != null)
            {
                await _instance.HideInternal(duration);
                Destroy();
            }
        }

        /// <summary>
        /// UIPopup의 VisibleState가 변할 때, 호출됩니다.
        /// </summary>
        /// <param name="visibleState"></param>
        protected void OnViewVisibleStateChanged(VisibleState visibleState)
        {
            if (visibleState == VisibleState.Disappearing)
            {
                HideInternal(_hideAnimDuration).Forget();
            }
        }

        /// <summary>
        /// UIPopup의 Popup 내부 동작입니다.
        /// </summary>
        /// <param name="duration">Animation 시간</param>
        /// <returns>UniTask</returns>
        protected virtual async UniTask ShowInternal(float duration)
        {
            List<UniTask> appearAnimJobs = new List<UniTask>();
            switch (this.VisibleState)
            {
                case VisibleState.Appeared:
                case VisibleState.Appearing:
                    return;
            }

            this.VisibleState = VisibleState.Appearing;

            // Show UI Animation 설정
            if (_contentCanvasGroup != null)
            {
                AddShowAnimationJobs(appearAnimJobs, duration, _contentCanvasGroup);
            }

            if (_backgroundCanvasGroup != null)
            {
                AddShowAnimationJobs(appearAnimJobs, duration, _backgroundCanvasGroup);
            }

            await UniTask.WhenAll(appearAnimJobs);

            this.VisibleState = VisibleState.Appeared;
        }

        /// <summary>
        /// UIPopup의 Dismiss 내부 동작입니다.
        /// </summary>
        /// <param name="duration">Animation 시간</param>
        /// <returns>UniTask</returns>
        protected virtual async UniTask HideInternal(float duration)
        {
            List<UniTask> appearAnimJobs = new List<UniTask>();
            switch (this.VisibleState)
            {
                case VisibleState.Disappeared:
                case VisibleState.Disappearing:
                    return;
            }

            this.VisibleState = VisibleState.Disappearing;

            // Show UI Animation 설정
            if (_contentCanvasGroup != null)
            {
                AddHideAnimationJobs(appearAnimJobs, duration, _contentCanvasGroup);
            }

            if (_backgroundCanvasGroup != null)
            {
                AddHideAnimationJobs(appearAnimJobs, duration, _backgroundCanvasGroup);
            }

            await UniTask.WhenAll(appearAnimJobs);

            this.VisibleState = VisibleState.Disappeared;
        }

        /// <summary>
        /// 내부 동작 Popup이 실행되면서 처리될 애니메이션 Job을 추가합니다.
        /// </summary>
        /// <param name="jobs">WhenAll이 실행될 UniTask list</param>
        /// <param name="duration">Animation 시간</param>
        /// <param name="target">타겟 CanvasGroup</param>
        protected virtual void AddShowAnimationJobs(List<UniTask> jobs, float duration, CanvasGroup target)
        {
            if (target != null)
            {
                var uiTransition = target.GetComponent<UITransitionBase>();
                jobs.Add(uiTransition.Show(duration));
            }
        }

        /// <summary>
        /// 내부 동작 Dismiss가 실행되면서 처리될 애니메이션 Job을 추가합니다.
        /// </summary>
        /// <param name="jobs">WhenAll이 실행될 UniTask list</param>
        /// <param name="duration">Animation 시간</param>
        /// <param name="target">타겟 CanvasGroup</param>
        protected virtual void AddHideAnimationJobs(List<UniTask> jobs, float duration, CanvasGroup target)
        {
            if (target != null)
            {
                var uiTransition = target.GetComponent<UITransitionBase>();
                jobs.Add(uiTransition.Hide(duration));
            }
        }

        /// <summary>
        /// 현재 UIPopup Object를 제거합니다.
        /// </summary>
        public static void Destroy()
        {
            if (_instance != null && _instance.VisibleState == VisibleState.Disappeared)
            {
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }
    }
}

