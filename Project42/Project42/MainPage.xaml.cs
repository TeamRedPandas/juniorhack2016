using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Project42
{
    public sealed partial class MainPage : Page
    {
        private readonly SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();

        private bool pageLoaded  = false;
        private bool frameLoaded = false;

        public MainPage()
        {
            InitializeComponent();
            systemNavigationManager.BackRequested += SystemNavigationManager_BackRequested;

            Loaded += (sender, e) =>
            {
                pageLoaded = true;
                TryInitialNavigation();
            };
        }

        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Fr_Main.CanGoBack)
            {
                e.Handled = true;
                Fr_Main.GoBack();
            }
        }

        private void Fr_Main_Loaded(object sender, RoutedEventArgs e)
        {
            frameLoaded = true;
            TryInitialNavigation();
        }

        private void Fr_Main_Navigated(object sender, NavigationEventArgs e)
        {
            systemNavigationManager.AppViewBackButtonVisibility = Fr_Main.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void TryInitialNavigation()
        {
            if (pageLoaded && frameLoaded)
            {
                Fr_Main.Navigate(typeof(TestPage));
            }
        }
    }
}
