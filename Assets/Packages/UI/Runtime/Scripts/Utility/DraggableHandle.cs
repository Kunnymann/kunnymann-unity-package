using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kunnymann.UI
{
    /// <summary>
    /// Draggable ui state
    /// </summary>
    public enum DraggableHandleState
    {
        None = 0,
        Hidden,
        Visible
    }

    /// <summary>
    /// 드래그 가능한 핸들 클래스
    /// </summary>
    public class DraggableHandle : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        /// <summary>
        /// Content RectTransform
        /// </summary>
        [SerializeField] private RectTransform _contentRectTransform;
        /// <summary>
        /// Animation을 이행할 최대 시간
        /// </summary>
        [SerializeField] private float _animationMaxDuration = 0.3f;

        /// <summary>
        /// 부모 RectTransform
        /// </summary>
        private RectTransform _parentRectTransform;
        /// <summary>
        /// 드래그 시, 포지셔닝 할 최대 위치
        /// </summary>
        private Vector2 _maxPosition;
        /// <summary>
        /// 드래그 시, 포지셔닝 할 최소 위치
        /// </summary>
        private Vector2 _minPosition;
        /// <summary>
        /// Animation 최대 시간
        /// </summary>
        private float _animationMaxWeight;
        /// <summary>
        /// Vertical 드래그 종료 위치
        /// </summary>
        private float _dragEndY;
        /// <summary>
        /// Vertical 드래그 시작 위치
        /// </summary>
        private float _dragStartY;
        /// <summary>
        /// Visible 상태로 전환할 최소 드래그 거리
        /// </summary>
        private float _visibleThreshold;

        /// <summary>
        /// 핸들 상태
        /// </summary>
        private DraggableHandleState _state = DraggableHandleState.None;
        /// <summary>
        /// 현재 핸들의 상태
        /// </summary>
        public DraggableHandleState State
        {
            get
            {
                return _state;
            }
            private set
            {
                if (_state != value)
                {
                    switch (value)
                    {
                        case DraggableHandleState.Hidden:
                            _state = DraggableHandleState.Hidden;
                            _parentRectTransform.anchoredPosition = _minPosition;
                            break;
                        case DraggableHandleState.Visible:
                            _state = DraggableHandleState.Visible;
                            _parentRectTransform.anchoredPosition = _maxPosition;
                            break;
                        default:
                            break;
                    }
                }

            }
        }

        /// <summary>
        /// 드래그 핸들과 콘텐츠를 초기화합니다
        /// </summary>
        private void Start()
        {
            _parentRectTransform = transform.parent.GetComponent<RectTransform>();
            _maxPosition = _parentRectTransform.anchoredPosition;
            _minPosition = _parentRectTransform.anchoredPosition - Vector2.up * (_parentRectTransform.rect.height - this.GetComponent<RectTransform>().rect.height);
            _animationMaxWeight = _maxPosition.y - _minPosition.y;

            _visibleThreshold = _parentRectTransform.rect.height * 0.1f;

            State = DraggableHandleState.Hidden;
        }

        /// <summary>
        /// 드래그 시작 이벤트
        /// </summary>
        /// <param name="eventData">이벤트 데이터</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragStartY = _parentRectTransform.anchoredPosition.y;

            StopCoroutine(SlideIn());
            StopCoroutine(SlideOut());
        }

        /// <summary>
        /// 드래그 이벤트
        /// </summary>
        /// <param name="eventData">이벤트 데이터</param>
        public void OnDrag(PointerEventData eventData)
        {
            _parentRectTransform.anchoredPosition += new Vector2(0, eventData.delta.y);

            if (_parentRectTransform.anchoredPosition.y < _minPosition.y)
            {
                _parentRectTransform.anchoredPosition = _minPosition;
            }
            else if (_parentRectTransform.anchoredPosition.y > _maxPosition.y)
            {
                _parentRectTransform.anchoredPosition = _maxPosition;
            }
        }

        /// <summary>
        /// 드래그 종료 이벤트
        /// </summary>
        /// <param name="eventData">이벤트 데이터</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            _dragEndY = _parentRectTransform.anchoredPosition.y;
            float dragDistance = Mathf.Abs(_dragStartY - _dragEndY);

            if (State == DraggableHandleState.Hidden)
            {
                if (dragDistance >= _visibleThreshold)
                {
                    StartCoroutine(SlideOut());
                }
                else
                {
                    StartCoroutine(SlideIn());
                }
            }
            else if (State == DraggableHandleState.Visible)
            {
                if (dragDistance >= _visibleThreshold)
                {
                    StartCoroutine(SlideIn());
                }
                else
                {
                    StartCoroutine(SlideOut());
                }
            }
        }

        /// <summary>
        /// 슬라이드 아웃하여 콘텐츠를 노출합니다
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator SlideOut()
        {
            float slideDuration = _animationMaxDuration * (Mathf.Abs((_animationMaxWeight - _dragEndY) / _animationMaxWeight));
            float elapsedTime = 0f;
            Vector2 _dragStartVector = new Vector2(0, _dragEndY);

            while (elapsedTime < slideDuration)
            {
                float t = elapsedTime / slideDuration;
                _parentRectTransform.anchoredPosition = Vector2.Lerp(_dragStartVector, _maxPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            State = DraggableHandleState.Visible;
        }

        /// <summary>
        /// 슬라이드 인하여 콘텐츠를 노출합니다
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator SlideIn()
        {
            float slideDuration = _animationMaxDuration * (Mathf.Abs((_animationMaxWeight - _dragEndY) / _animationMaxWeight));
            float elapsedTime = 0f;
            Vector2 _dragStartVector = new Vector2(0, _dragEndY);

            while (elapsedTime < slideDuration)
            {
                float t = elapsedTime / slideDuration;
                _parentRectTransform.anchoredPosition = Vector2.Lerp(_dragStartVector, _minPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            State = DraggableHandleState.Hidden;
        }
    }
}

