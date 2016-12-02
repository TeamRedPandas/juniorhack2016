using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Project42
{
    public abstract class PageBase : Page
    {
        public PageBase()
        {
            Name = ResourceLoader.GetForCurrentView().GetString($"{GetType().Name}/PageTitle").ToUpper();
        }
    }
}
