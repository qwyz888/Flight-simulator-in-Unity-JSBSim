using Cysharp.Threading.Tasks;

namespace Infrastructure.UI.Popups.Core
{
    public interface IConfirmationPopup : IPopup
    {
        public UniTask<ConfirmationPopupResult> ResultTask { get; }
    }
}