using FoodDiary.Models;
using FoodDiary.Services;
using FoodDiary.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDiary.ViewModels
{
    class WeightsViewModel : BaseViewModel
    {
        private Weight selectedWeight;
        public ObservableCollection<Weight> Weights { get; }
        public Command AddWeightCommand { get; }
        public Command LoadWeightsCommand { get; }
        public Command WeightTapped { get; }

        public WeightsViewModel()
        {
            Title = "Weights";
            Weights = new ObservableCollection<Weight>();
            WeightTapped = new Command<Weight>(OnWeightSelected);
            AddWeightCommand = new Command(OnAddWeight);
            LoadWeightsCommand = new Command(async () => await GetWeights());
        }
        public void OnAppearingAsync()
        {
            SelectedWeight = null;
            IsBusy = true;
        }

        async Task GetWeights()
        {
            IsBusy = true;

            try
            {
                Weights.Clear();
                var api = NetworkService.GetApiService();
                var weights = await api.GetWeights((string)Application.Current.Properties["token"],
                   (int)Application.Current.Properties["userId"]);

                Console.WriteLine("weight: " + weights.Count);
                weights.Sort((x, y) => DateTime.Compare(y.Date, x.Date));
                foreach (var weight in weights)
                {
                    Console.WriteLine("weight: " + weight.Mass);
                    weight.DateString = weight.Date.ToString("MM/dd/yyyy");
                    Weights.Add(weight);
                }
                
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
        public Weight SelectedWeight
        {
            get => selectedWeight;
            set
            {
                SetProperty(ref selectedWeight, value);
                OnWeightSelected(value);
            }
        }

        private async void OnAddWeight(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(NewWeightPage)}");
        }

        private async void OnWeightSelected(Weight weight)
        {
            if (weight == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"//{nameof(NewMealPage)}");
            await Shell.Current.GoToAsync($"//{nameof(UpdateWeightPage)}?{nameof(UpdateWeightViewModel.WeightId)}={weight.Id}");
        }
    }
}
