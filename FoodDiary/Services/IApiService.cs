using FoodDiary.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodDiary.Services
{
    public interface IApiService
    {
        [Get("/api/user")]
        Task<User> GetUser();

        [Post("/api/auth/login")]
        Task<string> PostLogin([Body] User user);

        [Post("/api/auth/register")]
        Task<string> PostRegister([Body] User user);

        [Get("/api/{request.userId}/meals/date/{request.date}")]
        Task<List<Meal>> GetMealsByDate(MealsByDateRequest request, [Authorize("Bearer")] string token);

        [Post("/api/{userId}/meals")]
        Task<string> PostMeal([Body] Meal meal, [Authorize("Bearer")] string token, string userId);
        
        [Get("/api/{userId}/meals/{mealId}")]
        Task<Meal> GetMeal([Authorize("Bearer")] string token, int userId, int mealId);

        [Put("/api/{userId}/meals/{mealId}")]
        Task<string> UpdateMeal([Body] Meal meal, [Authorize("Bearer")] string token, int userId, int mealId);

        [Delete("/api/{userId}/meals/{mealId}")]
        Task<string> DeleteMeal([Authorize("Bearer")] string token, int userId, int mealId);

        [Get("/api/{userId}/weights")]
        Task<List<Weight>> GetWeights([Authorize("Bearer")] string token, int userId);

        [Get("/api/{userId}/weights/{weightId}")]
        Task<Weight> GetWeight([Authorize("Bearer")] string token, int userId, int weightId);

        [Post("/api/{userId}/weights")]
        Task<string> AddWeight([Body] Weight weight, [Authorize("Bearer")] string token, int userId);

        [Delete("/api/{userId}/weights/{weightId}")]
        Task<string> DeleteWeight([Authorize("Bearer")] string token, int userId, int weightId);

        [Put("/api/{userId}/weights/{weightId}")]
        Task<string> UpdateWeight([Body] Weight weight, [Authorize("Bearer")] string token, int userId, int weightId);
        [Post("/api/payment")]
        Task<string> PostPayment([Body] Payment payment);
    }
}
