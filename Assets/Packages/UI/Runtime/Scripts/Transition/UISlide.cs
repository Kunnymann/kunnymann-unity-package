using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Kunnymann.UI
{
    /// <summary>
    /// UI Slide 애니메이션을 적용하는 UITransition
    /// </summary>
    public class UISlide : UITransitionBase
    {
        [SerializeField] private SlideDirections _slideDirection;
        
        private Tween tween;
        private Vector2 _positionToShow;
        private Vector2 _positionToHide;

        private RectTransform _parentRectTransform;
        private RectTransform _rectTransform;

        protected override void Initialize()
        {
            _parentRectTransform = transform.parent.GetComponent<RectTransform>();
            _rectTransform = GetComponent<RectTransform>();

            Vector2 relativePositionMin = _rectTransform.offsetMin;
            Vector2 relativePositionMax = _rectTransform.offsetMax;

            float width = _rectTransform.rect.width;
            float height = _rectTransform.rect.height;

            _positionToShow = _rectTransform.anchoredPosition;

            Vector2 delta = Vector2.zero;

            float _parentHalfWidth = _parentRectTransform != null ? _parentRectTransform.rect.width / 2 : 0;
            float _parentHalfHeight = _parentRectTransform != null ? _parentRectTransform.rect.height / 2 : 0;

            switch (_slideDirection)
            {
                case SlideDirections.LEFT:
                    delta = new Vector2(-width, 0);
                    break;
                case SlideDirections.RIGHT:
                    delta = new Vector2(width, 0);
                    break;
                case SlideDirections.UP:
                    delta = new Vector2(0, height);
                    break;
                case SlideDirections.DOWN:
                    delta = new Vector2(0, -height);
                    break;
            }
            _positionToHide = _positionToShow + delta;
        }

        protected override UniTask HideAnim(float duration)
        {
            tween = _rectTransform.DOAnchorPos(_positionToHide, duration).SetAutoKill();
            return tween.AsyncWaitForCompletion().AsUniTask();
        }

        protected override void HideWithoutAnim()
        {
            _rectTransform.anchoredPosition = _positionToHide;
        }

        protected override void KillAnim()
        {
            if (tween.active) tween.Kill(true);
        }

        protected override UniTask ShowAnim(float duration)
        {
            tween = _rectTransform.DOAnchorPos(_positionToShow, duration).SetAutoKill();
            return tween.AsyncWaitForCompletion().AsUniTask();
        }

        protected override void ShowWithoutAnim()
        {
            _rectTransform.anchoredPosition = _positionToShow;
        }
    }
}
