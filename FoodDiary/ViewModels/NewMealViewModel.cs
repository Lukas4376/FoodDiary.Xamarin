using FoodDiary.Models;
using FoodDiary.Services;
using FoodDiary.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FoodDiary.ViewModels
{
    class NewMealViewModel : BaseViewModel
    {
        public Command AddCommand { get; }
        public Command CancelCommand { get; }

        private string name = string.Empty;
        private int calories = 0;
        private int protein = 0;
        private int fat = 0;
        private int carb = 0;
        private int massOfPortion = 0;
        private int amountOfPortions = 0;
        private int type = 0;

        public NewMealViewModel()
        {
            AddCommand = new Command(OnAddClicked);
            CancelCommand = new Command(OnCancelClicked);
        }

        private int TypeMapper(string str)
        {
            if (str.Equals("Breakfast"))
                return 0;
            if (str.Equals("Lunch"))
                return 1;
            if (str.Equals("Dinner"))
                return 2;
            if (str.Equals("Snack"))
                return 3;
            return 0;
        }

        private async void OnAddClicked(object obj)
        {
            IsBusy = true;

            Meal mealToAdd = new Meal()
            {
                Name = Name,
                Calories = Calories,
                Protein = Protein,
                Fat = Fat,
                Carb = Carb,
                MassOfPortion = MassOfPortion,
                AmountOfPortions = AmountOfPortions,
                Type = Type,
                Date = (DateTime) Application.Current.Properties["date"]
            };

            try
            {
                var api = NetworkService.GetApiService();
                var json = await api.PostMeal(mealToAdd, 
                    Application.Current.Properties["token"].ToString(), 
                    Application.Current.Properties["userId"].ToString());
                await Shell.Current.GoToAsync($"//{nameof(DiaryPage)}");
                Name = string.Empty;
                Calories = 0;
                Protein = 0;
                Fat = 0;
                Carb = 0;
                MassOfPortion = 0;
                AmountOfPortions = 0;
                Type = 0;
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
            await Shell.Current.GoToAsync($"//{nameof(DiaryPage)}");
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public int Calories
        {
            get => calories;
            set => SetProperty(ref calories, value);
        }

        public int Protein
        {
            get => protein;
            set => SetProperty(ref protein, value);
        }

        public int Fat
        {
            get => fat;
            set => SetProperty(ref fat, value);
        }

        public int Carb
        {
            get => carb;
            set => SetProperty(ref carb, value);
        }

        public int MassOfPortion
        {
            get => massOfPortion;
            set => SetProperty(ref massOfPortion, value);
        }

        public int AmountOfPortions
        {
            get => amountOfPortions;
            set => SetProperty(ref amountOfPortions, value);
        }

        public int Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

    }
}
