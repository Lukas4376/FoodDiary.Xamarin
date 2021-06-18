using FoodDiary.Models;
using FoodDiary.Services;
using FoodDiary.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FoodDiary.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        public Command RegisterCommand { get; }
        public Command CancelCommand { get; }
        private string username = string.Empty;
        private string password = string.Empty;
        public RegisterViewModel()
        {
            RegisterCommand = new Command(OnRegisterClicked, ValidateSave);
            CancelCommand = new Command(OnCancelClicked);
            this.PropertyChanged +=
                (_, __) => RegisterCommand.ChangeCanExecute();
        }

        private bool ValidateSave(object arg)
        {
            return !String.IsNullOrWhiteSpace(username)
                && !String.IsNullOrWhiteSpace(password)
                && password.Length >= 8
                && password.Length <= 24;
        }

        private async void OnRegisterClicked(object obj)
        {
            IsBusy = true;

            User userToRegister = new User()
            {
                Username = Username,
                Password = Password
            };

            try
            {
                var api = NetworkService.GetApiService();
                var json = await api.PostRegister(userToRegister);
                var token = JsonConvert.DeserializeObject<LoginJson>(json);
                await Application.Current.MainPage.Navigation.PopAsync();
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                Username = string.Empty;
                Password = string.Empty;
                await Application.Current.MainPage.DisplayAlert("", "Registration successful", "Ok");
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

        private async void OnCancelClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
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
    }
}
