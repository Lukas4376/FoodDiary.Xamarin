using FoodDiary.Models;
using FoodDiary.Services;
using FoodDiary.Views;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDiary.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        private string username = string.Empty;
        private string password = string.Empty;

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
            Application.Current.Properties["token"] = null;
            Application.Current.Properties["userId"] = null;
        }

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        

        private async void OnLoginClicked(object obj)
        {
            IsBusy = true;

            User userToLogin = new User()
            {
                Username = Username,
                Password = Password
            };

            try
            {
                var api = NetworkService.GetApiService();
                var json = await api.PostLogin(userToLogin);
                var token = JsonConvert.DeserializeObject<LoginJson>(json);
                Application.Current.Properties["token"] = token.Token;
                Console.WriteLine(Application.Current.Properties["token"]);
                Application.Current.Properties["userId"] = token.Id;
                Console.WriteLine(Application.Current.Properties["userId"]);
                await Application.Current.MainPage.Navigation.PopAsync();
                await Shell.Current.GoToAsync($"//{nameof(DiaryPage)}");
                Username = string.Empty;
                Password = string.Empty;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnRegisterClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }
    }
}
