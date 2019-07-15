using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using NewsHeadline.Models.Entities;
using NewsHeadline.Models.ViewModels;
using NewsHeadline.Services;
using Newtonsoft.Json;
using RestSharp;

namespace NewsHeadline.Controllers
{
    /// <summary>
    /// Profile controller to allow a user to manage their profile settings
    /// </summary>
    public class ProfileController : Controller
    {
        private IUserSettingRepository _userSettingRepo;
        private INewsCategoryRepository _newsCategoryRepo;

        /// <summary>
        /// Constructor include the user setting and news category repositories
        /// </summary>
        /// <param name="userSettingRepo"></param>
        /// <param name="newsCategoryRepo"></param>
        public ProfileController(IUserSettingRepository userSettingRepo, INewsCategoryRepository newsCategoryRepo)
        {
            _userSettingRepo = userSettingRepo;
            _newsCategoryRepo = newsCategoryRepo;
        }
        
        /// <summary>
        /// Index view to all the user to manage their profile
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            var user = _userSettingRepo.Read(User.Identity.Name);
            
            // When the user is not found redirect to a login page
            if(user == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            // Get all the news categories that the user does not have
            var subtracted = _newsCategoryRepo.ReadAll().Where(n => !n.UserSettingNewsCategories.Select(news => news.NewsCategory.Name).Contains(n.Name)).ToList();

            // Get all the news categories of the user
            var newsCatList = _newsCategoryRepo.Read(user.Id).UserSettingNewsCategories.ToList();

            // Instantiate a new profile manager view model
            var userVM = new ProfileManageVM {
                Username = user.User.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Language = user.Language,
                Country = user.Country,
                SubtractedNewsCategoriesOptions = subtracted,
                UserSettingNewsCategories = newsCatList
            };
            
            return View(userVM);
        }

        /// <summary>
        /// Handle the updates to the user's setting profile
        /// Accepts a passed in profile view model
        /// and updates the settings according to appropriate changes.
        /// </summary>
        /// <param name="profileVM"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(ProfileManageVM profileVM)
        {
            // When the model state is valid
            if (ModelState.IsValid)
            {
                // Get the current user setting
                var currentUser = _userSettingRepo.Read(User.Identity.Name);
                // Get the user setting from the identity user found
                var userSetting = profileVM.GetInstance(currentUser);

                // Update the user's setting profile to what the user wants
                _userSettingRepo.Update(User.Identity.Name, userSetting, profileVM.NewsCategoryNames);
                return RedirectToAction("Index");
            }

            return View(profileVM);
        }

        /// <summary>
        /// Delete a category.
        /// Takes a category ID and deletes the associated category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            // Find the identity user
            var user = _userSettingRepo.Read(User.Identity.Name);

            // When there will be at least one news category setting if one is deleted
            if (user.UserSettingNewsCategories.Count() >= 2)
            {
                // Delete the supplied news category
                _newsCategoryRepo.Delete(id, user);
            }
            return RedirectToAction("Index", "Profile");
        }

        /// <summary>
        /// Displays a add news source view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View</returns>
        public IActionResult AddNewsSource(int id)
        {
            // Find the category from the supplied ID
            var category = _newsCategoryRepo.ReadUserSettingNewsCategory(id);
            // Find the identity user
            var user = _userSettingRepo.Read(User.Identity.Name);

            // When both are undefined, redirect the user to the index
            if (category == null || user == null)
            {
                return RedirectToAction("Index", "Profile");
            }

            // Instantiate a new add source view model
            var sourceVM = new AddSourceVM
            {
                ExistingNewSources = category.NewsSources,
                UserSettingNewsCategory = category,
                CategoryName = category.NewsCategory.Name,
                CategoryId = category.Id,
                Language = user.Language,
                Country = user.Country
            };

            return View(sourceVM);
        }

        /// <summary>
        /// Create a news source.
        /// Accepts a add source view model and updates
        /// the previous news source to match the updates
        /// </summary>
        /// <param name="sourceVM"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreateNewsSource(AddSourceVM sourceVM)
        {
            // Find the identity user
            var user = _userSettingRepo.Read(User.Identity.Name);
            // Find the user's associated news category
            var category = user.UserSettingNewsCategories.FirstOrDefault(u => u.NewsCategory.Name == sourceVM.CategoryName);

            // When both are undefined, return the user to the profile/index
            if (category == null || user == null)
            {
                return RedirectToAction("Index", "Profile");
            }

            // When the state of the model is valid
            if (ModelState.IsValid)
            {
                // Iterate over each news source
                foreach(var source in sourceVM.NewsSources)
                {
                    // Create the news source
                    _newsCategoryRepo.CreateNewsSource(source, category);
                }
                return RedirectToAction("AddNewsSource", "Profile", new { id = category.Id });
            }

            return View(sourceVM);
        }

        /// <summary>
        /// Delete a news source
        /// given a news source and category ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteNewsSource(int id, int categoryId)
        {
            // Find the user gien the username
            var user = _userSettingRepo.Read(User.Identity.Name);
            // Delete the news source given the news source ID and the user
            _newsCategoryRepo.DeleteNewsSource(id, user);
            
            return RedirectToAction("AddNewsSource", "Profile", new { id = categoryId });
        }

        /// <summary>
        /// Present a get sources view
        /// </summary>
        /// <param name="category"></param>
        /// <param name="language"></param>
        /// <param name="country"></param>
        /// <returns>A JSON object</returns>
        public IActionResult GetSources(string category, string language, string country)
        {
            // Given a category, language, and country, query the NewsAPI sources
            var client = new RestClient($"https://newsapi.org/v2/sources?apiKey=535ade99d34945edaf7c9277c8cbb351&category={category}&language={language}&country={country}");
            var response = client.Execute(new RestRequest());
            
            // Return a JSON list of sources
            return Json(response.Content);
        }
    }
}