using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Collections.Concurrent;
using System.Linq;

namespace Kunnymann.UI.Navigation
{
    /// <summary>
    /// ViewUnit을 관리하는 클래스
    /// </summary>
    public class UINavigation : MonoBehaviour
    {
        /// <summary>
        /// 초기화된 ViewUnit
        /// 없다면, 초기 고정 화면을 보여주지 않습니다
        /// </summary>
        [SerializeField] private ViewUnit _initializedView;

        private readonly List<ViewUnit> _viewUnits = new ();
        private ConcurrentQueue<(ViewUnit hide, ViewUnit show)> _viewTransitionQueue = new ();
        private Stack<ViewUnit> _viewStack = new ();

        private ViewUnit _previousViewUnit;
        /// <summary>
        /// 직전 Pop된 ViewUnit을 반환
        /// </summary>
        public ViewUnit PreviousViewUnit => _previousViewUnit;

        private ViewUnit _currentViewUnit;
        /// <summary>
        /// 현재 View를 반환
        /// </summary>
        public ViewUnit CurrentViewUnit => _currentViewUnit;
        
        /// <summary>
        /// 뷰가 전환될 때 호출할 이벤트
        /// </summary>
        public Action<ViewUnit> OnViewChanged;

        /// <summary>
        /// ViewUnit을 등록하며, 설정에 따라 초기 화면을 렌더링합니다
        /// </summary>
        private void Awake()
        {
            _viewUnits.AddRange(GetComponentsInChildren<ViewUnit>());

            // 초기화된 ViewUnit이 있다면, 첫 화면으로 바로 렌더링
            if (_initializedView != null && _initializedView.transform.IsChildOf(this.transform))
            {
                ViewUnit firstView = GetView<ViewUnit>(_initializedView.name);
                firstView.ShowImmediately();

                _viewStack.Push(firstView);
                _currentViewUnit = firstView;
            }
        }

        /// <summary>
        /// View들의 애니메이션 Queue를 초기화합니다
        /// </summary>
        private void Start()
        {
            Observable.EveryUpdate().Where(x => _viewTransitionQueue.Count > 0)
                .Subscribe(y =>
                {
                    if (_currentViewUnit == null || _currentViewUnit.VisibleState == VisibleState.Appeared)
                    {
                        _viewTransitionQueue.TryDequeue(out var dequeuedItem);
                        TransitViewUnit(dequeuedItem.hide, dequeuedItem.show);
                    }
                });
        }

        /// <summary>
        /// View 전환이 발생 시, Switching을 수행합니다
        /// </summary>
        /// <param name="hide"></param>
        /// <param name="show"></param>
        private async void TransitViewUnit(ViewUnit hide, ViewUnit show)
        {
            if (hide != null)
            {
                await hide.Hide();
            }

            _currentViewUnit = show;

            if (show != null)
            {
                await show.Show();
            }
        }

        /// <summary>
        /// 해당 Navigation에 등록된 View 객체를 가져옵니다
        /// </summary>
        /// <typeparam name="T">ViewUnit 클래스</typeparam>
        /// <param name="viewName">ViewUnit 게임오브젝트 이름</param>
        /// <returns>ViewUnit 객체</returns>
        public T GetView<T>(string viewName) where T : ViewUnit
        {
            ViewUnit result = _viewUnits.Find(x => x.name == viewName);

            if (result == null)
            {
                Debug.LogError($"{viewName}의 View를 찾을 수 없습니다");
                return null;
            }

            if (!typeof(T).IsAssignableFrom(result.GetType()))
            {
                Debug.LogError($"{viewName}의 타입 형식이 {result.GetType()}과 다릅니다");
                return null;
            }

            return result as T;
        }

        /// <summary>
        /// UIManager에 ViewUnit을 밀어넣습니다.
        /// History stack에 Push하고, View transition queue에 Transition을 밀어 넣습니다.
        /// </summary>
        /// <param name="viewName">View의 게임오브젝트 이름</param>
        /// <returns>View unit</returns>
        public T Push<T>(string viewName) where T : ViewUnit
        {
            ViewUnit show = _viewUnits.Find(x => x.name == viewName);

            if (show == null)
            {
                Debug.LogError($"{viewName}의 View를 찾을 수 없습니다");
                return null;
            }

            if (!typeof(T).IsAssignableFrom(show.GetType()))
            {
                Debug.LogError($"{viewName}의 타입 형식이 {show.GetType()}과 다릅니다");
                return null;
            }

            if (_currentViewUnit == show)
            {
                return show as T;
            }

            _viewStack.TryPeek(out ViewUnit hide);
            _viewStack.Push(show);
            _viewTransitionQueue.Enqueue((hide, show));
            return show as T;
        }

        /// <summary>
        /// UIManager에 현재 View를 뽑아냅니다.
        /// History stack에서 Pop하고, View transition queue에 Transition을 밀어 넣습니다.
        /// </summary>
        /// <returns>View unit</returns>
        public ViewUnit Pop()
        {
            if (_viewStack == null || _viewStack.Count == 0)
            {
                Debug.LogWarning("View가 더 이상 존재하지 않거나, View stack이 초기화되지 않았습니다");
                return null;
            }
            _viewStack.TryPop(out ViewUnit hide);
            _viewStack.TryPeek(out ViewUnit show);
            _viewTransitionQueue.Enqueue((hide, show));
            return hide;
        }

        /// <summary>
        /// UIManager에서 특정 View까지 모두 뽑아냅니다.
        /// </summary>
        /// <param name="viewName">View의 게임오브젝트 이름</param>
        /// <returns>View unit</returns>
        public ViewUnit PopTo(string viewName)
        {
            if (_viewStack == null || _viewStack.Count == 0)
            {
                Debug.LogWarning("View가 더 이상 존재하지 않거나, View stack이 초기화되지 않았습니다");
                return null;
            }

            if (!_viewStack.Any(view => view.name == viewName))
            {
                Debug.LogWarning($"{viewName}의 View를 찾을 수 없습니다");
                return null;
            }

            if (_viewStack.Last().name == viewName)
            {
                Debug.LogWarning($"{viewName}은 이미 현재 View입니다");
                return null;
            }

            _viewStack.TryPop(out ViewUnit hide);
            while (_viewStack.Count > 0)
            {
                _viewStack.TryPeek(out ViewUnit show);
                if (show.name == viewName)
                {
                    _viewTransitionQueue.Enqueue((hide, show));
                    break;
                }
                _viewStack.Pop();
            }
            return hide;
        }

        /// <summary>
        /// UIManager에서 RootView까지 모두 뽑아냅니다.
        /// </summary>
        /// <returns>Root view</returns>
        public ViewUnit PopToRoot()
        {
            if (_viewStack == null || _viewStack.Count == 0)
            {
                Debug.LogWarning("View가 더 이상 존재하지 않거나, View stack이 초기화되지 않았습니다");
                return null;
            }

            if (_viewStack.Count < 2)
            {
                Debug.LogWarning("View가 현재 RootView만 있습니다");
                _viewStack.TryPeek(out ViewUnit root);
                return root;
            }

            _viewStack.TryPop(out ViewUnit hide);
            while (_viewStack.Count > 1)
            {
                _viewStack.Pop();
            }
            _viewStack.TryPeek(out ViewUnit show);
            _viewTransitionQueue.Enqueue((hide, show));

            return show;
        }
    }
}