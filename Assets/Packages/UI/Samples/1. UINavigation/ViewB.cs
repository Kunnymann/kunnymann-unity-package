using Kunnymann.UI.Navigation;
using UnityEngine;
using UnityEngine.UI;

namespace Kunnymann.UI.Sample
{
    public class ViewB : ViewUnit
    {
        [SerializeField] private UINavigation _navigation;
        [SerializeField] private Button _button;

        protected override void Start()
        {
            base.Start();

            _button.onClick.AddListener(OnClickButton);
        }

        private void OnClickButton()
        {
            _navigation.Push<ViewC>("View-C");
        }
    }
}
