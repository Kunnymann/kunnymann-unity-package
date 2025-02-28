using Kunnymann.Base.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kunnymann.UI.Popup
{
    /// <summary>
    /// UIPopup Prefab들을 관리하는 Container
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public class PopupContainer : MonoBehaviourSingleton<PopupContainer>
    {
        /// <summary>
        /// UIPopup 프리팹 리스트
        /// </summary>
        [SerializeField] private List<PopupUnit> _uiPopupList;

        /// <summary>
        /// UIPopup을 리스트로부터 찾아와, Instantiate하여 반환합니다.
        /// </summary>
        /// <param name="popupName">UIPopup 프리팹 이름</param>
        /// <returns>UIPopup</returns>
        internal PopupUnit GetUIPopup(string popupName)
        {
            var popup = _uiPopupList.SingleOrDefault(found => found.name == popupName);
            if (popup == null)
            {
                Debug.LogError($"{popupName}의 Popup UI를 찾을 수 없습니다.");
                return null;
            }

            popup = Instantiate(popup, transform, false);
            popup.HideImmediately();
            return popup;
        }
    }
}

