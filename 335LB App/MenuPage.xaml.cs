using Microsoft.Maui.Controls;

namespace _335LB_App
{
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.MenuViewModel();
        }
    }
}
