using FoodDiary.Models;
using FoodDiary.Services;
using FoodDiary.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FoodDiary.ViewModels
{
    [QueryProperty(nameof(MealId), nameof(MealId))]
    class MealDetailViewModel : BaseViewModel
    {
        public Command UpdateCommand { get; }
        public Command DeleteCommand { get; }
        public Command CancelCommand { get; }

        private int mealId;
        private string name;
        private int calories;
        private int protein;
        private int fat;
        private int carb;
        private int massOfPortion;
        private int amountOfPortions;
        private int type;

        public MealDetailViewModel()
        {
            Title = "Meal details";
            UpdateCommand = new Command(OnUpdateClicked);
            DeleteCommand = new Command(OnDeleteClicked);
            CancelCommand = new Command(OnCancelClicked);
        }

        private async void OnCancelClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(DiaryPage)}");
        }

        private async void OnDeleteClicked(object obj)
        {
            bool answer = await Application.Current.MainPage.
                DisplayAlert("Warning", "Delete the meal?", "Yes", "No");
            
            if (!answer)
                return;

            try
            {
                var api = NetworkService.GetApiService();
                var json = await api.DeleteMeal(Application.Current.Properties["token"].ToString(),
                    (int)Application.Current.Properties["userId"], mealId);
                await Shell.Current.GoToAsync($"//{nameof(DiaryPage)}");
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

            Meal mealToUpdate = new Meal()
            {
                Name = Name,
                Calories = Calories,
                Protein = Protein,
                Fat = Fat,
                Carb = Carb,
                MassOfPortion = MassOfPortion,
                AmountOfPortions = AmountOfPortions,
                Type = Type,
                Date = (DateTime)Application.Current.Properties["date"]
            };

            try
            {
                var api = NetworkService.GetApiService();
                var json = await api.UpdateMeal(mealToUpdate,
                    Application.Current.Properties["token"].ToString(),
                    (int) Application.Current.Properties["userId"], mealId);
                await Shell.Current.GoToAsync($"//{nameof(DiaryPage)}");
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

        public async void LoadMealId(int mealId)
        {
            try
            {
                var api = NetworkService.GetApiService();
                var meal = await api.GetMeal((string)Application.Current.Properties["token"], 
                    (int)Application.Current.Properties["userId"],
                    mealId);
                Name = meal.Name;
                Calories = meal.Calories;
                Carb = meal.Carb;
                Protein = meal.Protein;
                Fat = meal.Fat;
                MassOfPortion = meal.MassOfPortion;
                AmountOfPortions = meal.AmountOfPortions;
                Type = meal.Type;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");
            }
        }

        public int MealId
        {
            get
            {
                return mealId;
            }
            set
            {
                mealId = value;
                LoadMealId(value);
            }
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
