using Cysharp.Threading.Tasks;
using Infrastructure.Services.Window.Core;
using Infrastructure.UI.Popups.Core;
using UI.Common;
using UnityEngine;
using VContainer;

namespace Infrastructure.UI.Buttons.Test
{
    public class ShowContinuationPopupButton : BaseButton
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
                .CreateWindow(WindowID.ContinuationPopup)
                .ContinueWith(window =>
                {
                    IContinuationPopup continuationPopup = (IContinuationPopup)window;

                    continuationPopup.Show();

                    continuationPopup.ContinueTask.ContinueWith(() => Debug.Log("Continue")).Forget();
                })
                .Forget();
        }
    }
}