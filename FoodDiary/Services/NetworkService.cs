using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDiary.Services
{
    public static class NetworkService
    {
        public static IApiService apiService;
        private static readonly string baseUrl = "http://10.0.2.2:5000";
        public static IApiService GetApiService()
        {
            apiService = RestService.For<IApiService>(baseUrl);
            return apiService;
        }
    }
}
