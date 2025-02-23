using System;
using UnityEngine.Events;

namespace Kunnymann.UI.Popup
{
    /// <summary>
    /// Popup 확장 메소드 클래스
    /// </summary>
    public static class PopupUtility
    {
        /// <summary>
        /// UIPopup을 구성하고 있는 TextFields에 텍스트를 설정합니다.
        /// </summary>
        /// <typeparam name="T">UIPopup 클래스</typeparam>
        /// <param name="uiPopup">UIPopup</param>
        /// <param name="texts">TextField에 들어갈 값</param>
        /// <returns>UIPopup</returns>
        public static T SetText<T>(this T uiPopup, params string[] texts) where T : PopupUnit
        {
            if (texts.Length == 0) return uiPopup;
            if (uiPopup.TextFields.Count == 0) return uiPopup;

            for (int idx = 0; idx < uiPopup.TextFields.Count; idx++)
            {
                if (idx > texts.Length - 1) break;
                uiPopup.TextFields[idx].text = texts[idx];
            }
            return uiPopup;
        }

        /// <summary>
        /// Popup에 지정된 TextFields에 LocalizedText를 설정합니다.
        /// </summary>
        /// <typeparam name="T">UIPopup 클래스</typeparam>
        /// <param name="uiPopup">UIPopup</param>
        /// <returns>UIPopup</returns>
        [Obsolete("Not yet developed")]
        public static T SetLocalizedText<T>(this T uiPopup, bool forceEmpty = true) where T : PopupUnit
        {
            if (uiPopup.TextFields.Count == 0) return uiPopup;

            for (int idx = 0; idx < uiPopup.TextFields.Count; idx++)
            {
                // Todo : Locale 작업 어떻게 수행할 것인지 확인
                /*
                Localization.LocalizationManager.Instance.GetLocalizedText(uiPopup.TextFields[idx].text, out string value, forceEmpty);
                uiPopup.TextFields[idx].text = value;
                */
            }
            return uiPopup;
        }

        /// <summary>
        /// UIPopup을 구성하고 있는 Button에 이벤트를 등록합니다.
        /// </summary>
        /// <typeparam name="T">UIPopup 클래스</typeparam>
        /// <param name="uiPopup">UIPopup</param>
        /// <param name="actions">Button에 할당될 이벤트</param>
        /// <returns>UIPopup</returns>
        public static T SetButtonEvent<T>(this T uiPopup, params UnityAction[] actions) where T : PopupUnit
        {
            if (actions.Length == 0) return uiPopup;
            if (uiPopup.Buttons.Count == 0) return uiPopup;

            for (int idx = 0; idx < uiPopup.Buttons.Count; idx++)
            {
                if (idx > actions.Length - 1) break;
                uiPopup.Buttons[idx].onClick.AddListener(actions[idx]);
            }

            return uiPopup;
        }

        /// <summary>
        /// UIPopup의 Dependency를 설정합니다.
        /// </summary>
        /// <typeparam name="T">UIPopup 클래스</typeparam>
        /// <param name="uiPop">UIPopup</param>
        /// <param name="view">View</param>
        /// <returns>UIPopup</returns>
        public static T SetDependencyOnView<T>(this T uiPop, ViewUnit view) where T : PopupUnit
        {
            uiPop.View = view;
            return uiPop;
        }
    }
}