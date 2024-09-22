using Microsoft.Maui.Controls;

namespace _335LB_App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MenuPage()); // Start with the MenuPage
        }
    }
}
