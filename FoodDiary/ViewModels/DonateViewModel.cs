using FoodDiary.Models;
using FoodDiary.Services;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FoodDiary.ViewModels
{
    class DonateViewModel : BaseViewModel
    {
        public Command DonateCommand { get; }
        private string creditCardNumber = string.Empty;
        private string mM = string.Empty;
        private string yY = string.Empty;
        private string cVV = string.Empty;
        private string amount = string.Empty;

        public DonateViewModel()
        {
            DonateCommand = new Command(OnDonateClicked, ValidateSave);
            this.PropertyChanged +=
                (_, __) => DonateCommand.ChangeCanExecute();
        }
        
        private bool ValidateSave(object arg)
        {
            return !String.IsNullOrWhiteSpace(creditCardNumber)
                && !String.IsNullOrWhiteSpace(mM)
                && !String.IsNullOrWhiteSpace(yY)
                && !String.IsNullOrWhiteSpace(cVV)
                && creditCardNumber.Length == 16
                && cVV.Length == 3
                && yY.Length == 2
                && mM.Length == 2
                && amount.Length >= 1;
        }

        private async void OnDonateClicked(object obj)
        {
            try
            {
                var token = CreateToken(creditCardNumber, mM, yY, cVV);
                var payment = new Payment();
                payment.Amount = int.Parse(Amount);
                payment.Token = token;
                var api = NetworkService.GetApiService();
                var json = await api.PostPayment(payment);
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thanks!", "", "Ok");
            }
            catch (Exception e)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");
            }
        }

        public string CreateToken(string cardNumber, string cardExpMonth, string cardExpYear, string cardCVC)
        {
            StripeConfiguration.ApiKey = "pk_test_51I7gdcKhTDjEMF3pCbMSERK9k0VlKifvGUiMYperqvOJycUcktKU21r1ogbLuXnl4lxlP3AHR5xQ2DQSjt6lxCp800k2868kLH";

            var tokenOptions = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Number = cardNumber,
                    ExpMonth = Convert.ToInt64(cardExpMonth),
                    ExpYear = Convert.ToInt64(cardExpYear),
                    Cvc = cardCVC,
                },
            };

            var service = new TokenService();
            var stripeToken = service.Create(tokenOptions);

            return stripeToken.Id;
        }

        public string CreditCardNumber
        {
            get => creditCardNumber;
            set => SetProperty(ref creditCardNumber, value);
        }

        public string MM
        {
            get => mM;
            set => SetProperty(ref mM, value);
        }

        public string YY
        {
            get => yY;
            set => SetProperty(ref yY, value);
        }

        public string CVV
        {
            get => cVV;
            set => SetProperty(ref cVV, value);
        }

        public string Amount
        {
            get => amount;
            set => SetProperty(ref amount, value);
        }
    }
}
