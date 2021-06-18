using FoodDiary.Models;
using FoodDiary.Services;
using FoodDiary.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDiary.ViewModels
{
    public class DiaryViewModel : BaseViewModel
    {
        private Meal _selectedMeal;
        public ObservableCollection<Meal> MealsBreakfast { get; }
        public ObservableCollection<Meal> MealsLunch { get; }
        public ObservableCollection<Meal> MealsDinner { get; }
        public ObservableCollection<Meal> MealsSnack { get; }
        public Command LoadMealsCommand { get; }
        public Command AddMealCommand { get; }
        public Command<Meal> MealTapped { get; }
        private DateTime selectedDate;
        private string summary = "Calories: 0 kcal  Proteins: 0 g  Fat: 0 g  Carbs: 0 g";

        public DiaryViewModel()
        {
            Title = "Diary";
            MealsBreakfast = new ObservableCollection<Meal>();
            MealsLunch = new ObservableCollection<Meal>();
            MealsDinner = new ObservableCollection<Meal>();
            MealsSnack = new ObservableCollection<Meal>();
            //LoadMealsCommand = new get meals
            MealTapped = new Command<Meal>(OnMealSelected);

            AddMealCommand = new Command(OnAddMeal);
            selectedDate = DateTime.Now;
            Application.Current.Properties["date"] = selectedDate;
        }

        public async Task OnAppearingAsync()
        {
            SelectedMeal = null;
            await GetMeals(selectedDate);
        }

        public Meal SelectedMeal
        {
            get => _selectedMeal;
            set
            {
                SetProperty(ref _selectedMeal, value);
                OnMealSelected(value);
            }
        }

        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                SetProperty(ref selectedDate, value);
                OnDateSelected(value);
            }
        }

        public string Summary
        {
            get => summary;
            set
            {
                SetProperty(ref summary, value);
            }
        }

        private async void OnAddMeal(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(NewMealPage)}");
        }

        async void OnDateSelected(DateTime date)
        {
            await GetMeals(date);
            Application.Current.Properties["date"] = date;
        }

        async Task GetMeals(DateTime date)
        {
            IsBusy = true;

            try
            {
                MealsBreakfast.Clear();
                MealsLunch.Clear();
                MealsDinner.Clear();
                MealsSnack.Clear();
                MealsByDateRequest req = new MealsByDateRequest();
                req.date = date.ToString("yyyy-MM-dd");
                req.userId = (int)Application.Current.Properties["userId"];
                var api = NetworkService.GetApiService();
                var meals = await api.GetMealsByDate(req, (string)Application.Current.Properties["token"]);
                int cal = 0;
                int protein = 0;
                int fat = 0;
                int carb = 0;

                foreach (var meal in meals)
                {
                    if (meal.Type == 0)
                        MealsBreakfast.Add(meal);
                    if (meal.Type == 1)
                        MealsLunch.Add(meal);
                    if (meal.Type == 2)
                        MealsDinner.Add(meal);
                    if (meal.Type == 3)
                        MealsSnack.Add(meal);

                    cal += meal.Calories;
                    protein += meal.Protein;
                    fat += meal.Fat;
                    carb += meal.Carb;
                }

                Summary = " Calories: " + cal + "kcal  \n Proteins: " + protein + "g  Fat: " + fat + "g  Carbs: " + carb + "g";
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


        async void OnMealSelected(Meal meal)
        {
            if (meal == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"//{nameof(NewMealPage)}");
            await Shell.Current.GoToAsync($"//{nameof(MealDetailPage)}?{nameof(MealDetailViewModel.MealId)}={meal.Id}");
        }
    }
}
