using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Kunnymann.UI
{
    /// <summary>
    /// UI를 구성하는 View의 최소 단위
    /// </summary>
    public class ViewUnit : MonoBehaviour
    {
        /// <summary>
        /// Show duration
        /// </summary>
        [SerializeField] private float _showAnimDuration;
        /// <summary>
        /// Hide duration
        /// </summary>
        [SerializeField] private float _hideAnimDuration;
        
        /// <summary>
        /// View의 RectTransform
        /// </summary>
        private RectTransform _rectTransform;

        /// <summary>
        /// View 예하의 TransitionBase 컨테이너
        /// </summary>
        private List<UITransitionBase> _uiTransitions;
        /// <summary>
        /// TransitionBase 컨테이너를 반환하며, 없다면 초기화를 진행합니다
        /// </summary>
        internal List<UITransitionBase> UITransitions
        {
            get
            {
                if (_uiTransitions == null)
                {
                    _uiTransitions = GetComponentsInChildren<UITransitionBase>().ToList();
                }

                // TODO : 이거 예하에 UITransitionBase가 없으면 어떻게 할 지 고민해봐야 함
                if (_uiTransitions.Count == 0)
                {
                    _uiTransitions.Add(gameObject.AddComponent<UIFadeInOut>());
                }

                return _uiTransitions;
            }
        }

        /// <summary>
        /// TransitionBase 중, 가장 첫 번째 TransitionBase의 CanvasGroup을 반환
        /// </summary>
        protected CanvasGroup CanvasGroup => UITransitions.FirstOrDefault().CanvasGroup;
        /// <summary>
        /// TransitionBase 중, 가장 첫 번째 VisibleState를 반환
        /// </summary>
        public VisibleState VisibleState => UITransitions.FirstOrDefault().VisibleState;
        /// <summary>
        /// VisibleState 전이 이벤트
        /// </summary>
        public UnityEvent<VisibleState> ChangedVisibleState => UITransitions.FirstOrDefault().ChangedVisibleState;

        /// <summary>
        /// 루트 캔버스에 View들을 집합 및 초기화합니다
        /// </summary>
        protected virtual void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.anchoredPosition = Vector2.zero;

            UITransitions.ForEach(transition => transition.HideImmediately());
        }

        /// <summary>
        /// 등록된 OnChangedVisibleState 이벤트에 대한 리스너를 추가합니다
        /// </summary>
        protected virtual void Start()
        {
            UITransitions.FirstOrDefault().ChangedVisibleState.AddListener(OnChangedVisibleState);
        }

        /// <summary>
        /// VisibleState에 따라 호출될 이벤트
        /// </summary>
        /// <param name="visibleState">VisibleState</param>
        private void OnChangedVisibleState(VisibleState visibleState)
        {
            switch (visibleState)
            {
                case VisibleState.Appeared:
                    OnShow();
                    break;
                case VisibleState.Appearing:
                    OnShowing();
                    break;
                case VisibleState.Disappeared:
                    OnHide();
                    break;
                case VisibleState.Disappearing:
                    OnHiding();
                    break;
            }
        }

        /// <summary>
        /// Show 애니메이션을 수행합니다
        /// </summary>
        /// <returns>UniTask</returns>
        internal protected virtual async UniTask Show()
        {
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            if (UITransitions != null)
            {
                List<UniTask> tasks = new List<UniTask>();
                UITransitions.ForEach(transition => tasks.Add(transition.Show(_showAnimDuration)));
                await UniTask.WhenAll(tasks);
            }
        }

        /// <summary>
        /// Hide 애니메이션을 수행합니다
        /// </summary>
        /// <returns>UniTask</returns>
        internal protected virtual async UniTask Hide()
        {
            if (UITransitions != null)
            {
                List<UniTask> tasks = new List<UniTask>();
                UITransitions.ForEach(transition => tasks.Add(transition.Hide(_hideAnimDuration)));
                await UniTask.WhenAll(tasks);
            }
        }

        /// <summary>
        /// 즉시 Show를 수행합니다
        /// </summary>
        internal protected virtual void ShowImmediately()
        {
            if (UITransitions != null)
            {
                UITransitions.ForEach(transition => transition.Show(0).Forget());
            }
        }

        /// <summary>
        /// 즉시 Hide를 수행합니다
        /// </summary>
        internal protected virtual void HideImmediately()
        {
            if (UITransitions != null)
            {
                UITransitions.ForEach(transition => transition.Hide(0).Forget());
            }
        }

        /// <summary>
        /// Show에 호출될 리스너 포맷 함수
        /// </summary>
        internal protected virtual void OnShow()
        {
            this.CanvasGroup.interactable = true;
        }
        /// <summary>
        /// Showing에 호출될 리스너 포멧 함수
        /// </summary>
        internal protected virtual void OnShowing()
        {
            this.CanvasGroup.interactable = false;
        }

        /// <summary>
        /// Hide에 호출될 리스너 포맷 함수
        /// </summary>
        internal protected virtual void OnHide()
        {
            this.CanvasGroup.interactable = false;
        }

        /// <summary>
        /// Hiding에 호출될 리스너 포멧 함수
        /// </summary>
        internal protected virtual void OnHiding()
        {
            this.CanvasGroup.interactable = false;
        }
    }
}