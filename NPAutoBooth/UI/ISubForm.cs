using NPCommon;

namespace NPAutoBooth.UI
{
    public interface ISubForm
    {
        event EventHandlerAddCtrl OnAddCtrl;

        void OpenView<T>(NPSYS.FormType pFormType, T param);

        void CloseView();

        void OpenViewBeforeInfo(NPSYS.FormType pFormType);

        void CloseViewBeforeInfo();

    }
}
