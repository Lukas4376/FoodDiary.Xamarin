using FoodDiary.Models;
using FoodDiary.Services;
using FoodDiary.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FoodDiary.ViewModels
{
    [QueryProperty(nameof(WeightId), nameof(WeightId))]
    class UpdateWeightViewModel : BaseViewModel
    {
        public Command UpdateCommand { get; }
        public Command DeleteCommand { get; }
        public Command CancelCommand { get; }

        private int weightId;
        private float mass = 0;
        private DateTime date = DateTime.Now;

        public UpdateWeightViewModel()
        {
            UpdateCommand = new Command(OnUpdateClicked);
            DeleteCommand = new Command(OnDeleteClicked);
            CancelCommand = new Command(OnCancelClicked);
        }

        private async void OnCancelClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(WeightsPage)}");
        }

        private async void OnDeleteClicked(object obj)
        {
            bool answer = await Application.Current.MainPage.
                DisplayAlert("Warning", "Delete the weight?", "Yes", "No");

            if (!answer)
                return;

            try
            {
                var api = NetworkService.GetApiService();
                var json = await api.DeleteWeight(Application.Current.Properties["token"].ToString(),
                    (int)Application.Current.Properties["userId"], weightId);
                await Shell.Current.GoToAsync($"//{nameof(WeightsPage)}");
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

        private async void OnUpdateClicked(object obj)
        {
            IsBusy = true;

            Weight weightToUpdate = new Weight()
            {
                Mass = Mass,
                Date = Date
            };

            try
            {
                var api = NetworkService.GetApiService();
                var json = await api.UpdateWeight(weightToUpdate,
                    Application.Current.Properties["token"].ToString(),
                    (int)Application.Current.Properties["userId"], weightId);
                await Shell.Current.GoToAsync($"//{nameof(WeightsPage)}");
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

        public async void LoadWeightId(int weightId)
        {
            try
            {
                var api = NetworkService.GetApiService();
                var weight = await api.GetWeight((string)Application.Current.Properties["token"],
                    (int)Application.Current.Properties["userId"],
                    weightId);
                Mass = weight.Mass;
                Date = weight.Date;
                
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");
            }
        }

        public int WeightId
        {
            get
            {
                return weightId;
            }
            set
            {
                weightId = value;
                LoadWeightId(value);
            }
        }

        public float Mass
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
