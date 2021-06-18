﻿using FoodDiary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodDiary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MealDetailPage : ContentPage
    {
        public MealDetailPage()
        {
            InitializeComponent();
            this.BindingContext = new MealDetailViewModel();
        }
    }
}