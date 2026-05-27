using Cysharp.Threading.Tasks;
using Infrastructure.Services.Window.Core;
using Infrastructure.UI.Popups.Core;
using UI.Common;
using UnityEngine;
using VContainer;

namespace Infrastructure.UI.Buttons.Test
{
    public class ShowConfirmationPopupButton : BaseButton
    {
        private IWindowService _windowService;

        [Inject]
        public void Construct(IWindowService windowService)
        {
            _windowService = windowService;
        }

        protected override void OnClick()
        {
            _windowService
                .CreateWindow(WindowID.ConfirmationPopup)
                .ContinueWith(window =>
                {
                    IConfirmationPopup continuationPopup = (IConfirmationPopup)window;

                    continuationPopup.Show();

                    continuationPopup.ResultTask.ContinueWith(result => Debug.Log("Result: " + result)).Forget();
                })
                .Forget();
        }
    }
}