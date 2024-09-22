using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace _335LB_App.ViewModels
{
    public class MenuViewModel
    {
        public ICommand EnterLaunchpadCommand { get; private set; }

        public MenuViewModel()
        {
            EnterLaunchpadCommand = new Command(OnEnterLaunchpadClicked);
        }

        private async void OnEnterLaunchpadClicked()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
        }
    }
}
