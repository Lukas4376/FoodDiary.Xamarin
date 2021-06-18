using FoodDiary.Models;
using FoodDiary.Services;
using FoodDiary.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FoodDiary.ViewModels
{
    class NewWeightViewModel : BaseViewModel 
    {
        public Command AddCommand { get; }
        public Command CancelCommand { get; }
        private int mass = 0;
        private DateTime date = DateTime.Now;

        public NewWeightViewModel()
        {
            AddCommand = new Command(OnAddClicked);
            CancelCommand = new Command(OnCancelClicked);
        }

        private async void OnAddClicked(object obj)
        {
            IsBusy = true;

            Weight weightToAdd = new Weight()
            {
                Mass = Mass,
                Date = Date
            };

            try
            {
                var api = NetworkService.GetApiService();
                var json = await api.AddWeight(weightToAdd,
                    Application.Current.Properties["token"].ToString(),
                    (int) Application.Current.Properties["userId"]);
                await Shell.Current.GoToAsync($"//{nameof(WeightsPage)}");
                mass = 0;
                date = DateTime.Now;
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
            await Shell.Current.GoToAsync($"//{nameof(WeightsPage)}");
        }

        public int Mass
        {
            get => mass;
            set => SetProperty(ref mass, value);
        }

        public DateTime Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }
    }
}
