using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Kunnymann.UI
{
    /// <summary>
    /// UI Friendly Popup 애니메이션을 적용하는 UITransition
    /// </summary>
    public class UIFriendlyPopIn : UITransitionBase
    {
        [SerializeField] [Range(0, 0.9f)] private float _divergingScale = 0.2f;

        private Tween tween;
        private RectTransform _rectTransform;

        protected override void Initialize()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        protected override UniTask HideAnim(float duration)
        {
            float divergingDuration = duration * _divergingScale;
            float convergingDuration = duration - divergingDuration;
            tween = DOTween.Sequence().Append(_rectTransform.DOScale(1.1f, divergingDuration))
                                      .Append(_rectTransform.DOScale(0, convergingDuration))
                                      .SetAutoKill();
            return tween.AsyncWaitForCompletion().AsUniTask();
        }

        protected override void HideWithoutAnim()
        {
            _rectTransform.localScale = Vector3.zero;
        }

        protected override void KillAnim()
        {
            if (tween.active) tween.Kill(true);
        }

        protected override UniTask ShowAnim(float duration)
        {
            float divergingDuration = duration * _divergingScale;
            float convergingDuration = duration - divergingDuration;
            tween = DOTween.Sequence().Append(_rectTransform.DOScale(1.1f, convergingDuration))
                                      .Append(_rectTransform.DOScale(1, divergingDuration))
                                      .SetAutoKill();
            return tween.AsyncWaitForCompletion().AsUniTask();
        }

        protected override void ShowWithoutAnim()
        {
            _rectTransform.localScale = Vector3.one;
        }
    }
}