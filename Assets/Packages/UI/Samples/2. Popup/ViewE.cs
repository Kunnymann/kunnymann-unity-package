using Cysharp.Threading.Tasks;
using Kunnymann.UI.Navigation;
using Kunnymann.UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace Kunnymann.UI.Sample
{
    public class ViewE : ViewUnit
    {
        [SerializeField] private UINavigation _navigation;
        [SerializeField] private Button _button;
        [SerializeField] private Button _popupButton;

        protected override void Start()
        {
            base.Start();

            _button.onClick.AddListener(OnClickButton);
            _popupButton.onClick.AddListener(OnClickPopupButton);
        }

        private void OnClickButton()
        {
            _navigation.Push<ViewD>("View-D");
        }

        private void OnClickPopupButton()
        {
            var popup = PopupUnit.GetUIPopup("SamplePopup");

            popup.SetButtonEvent(() => popup.Hide().Forget())
                 .SetText("You can change popup text!").Show(1).Forget();
        }
    }
}
