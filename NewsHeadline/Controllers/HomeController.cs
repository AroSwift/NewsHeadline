using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsHeadline.Models;
using NewsHeadline.Models.ViewModels;
using NewsHeadline.Services;
using Newtonsoft.Json;

namespace NewsHeadline.Controllers
{
    public class HomeController : Controller
    {

        private IUserSettingRepository _userSettingRepo;
        private INewsCategoryRepository _newsCategoryRepo;

        public HomeController(IUserSettingRepository userSettingRepo, INewsCategoryRepository newsCategoryRepo)
        {
            _userSettingRepo = userSettingRepo;
            _newsCategoryRepo = newsCategoryRepo;
        }

        /// <summary>
        /// Index view to display the news.
        /// Provides a profile manage view model
        /// with important information about a user
        /// so that their settings can be used to
        /// filter the news sources selected
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            // Find the user
            var user = _userSettingRepo.Read(User.Identity.Name);

            // When the user is not found
            if (user == null)
            {
                return LocalRedirect("/Identity/Account/Register");
            } else
            {
                // Create a dictionary with a key that has several attributes
                var categorySources = new Dictionary<string, List<string>>();

                // Iterate over each category
                foreach (var category in user.UserSettingNewsCategories)
                {
                    // Set the source list
                    var sourceList = new List<string>();
                    // Iterate over each source
                    foreach (var source in category.NewsSources.Select(s => s.SourceId))
                    {
                        // Add the source associated with the category to the source list
                        sourceList.Add(source);
                    }

                    // When a category at least has 1 source
                    if (sourceList.Count() > 0)
                    {
                        // Add the category and it's sources to the category source list
                        categorySources.Add(category.NewsCategory.Name, sourceList);
                    }
                }
                
                // Instantiate a new news list view model
                NewsListVM listVM = new NewsListVM
                {
                    UserSettingNewsCategories = user.UserSettingNewsCategories,
                    Country = user.Country,
                    CategorySources = JsonConvert.SerializeObject(categorySources)
                };
                return View(listVM);
            }
        }

        /// <summary>
        /// Present an about view - explain the purpose of the application and how it works
        /// </summary>
        /// <returns></returns>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Present an about developer view - describe the person who wrote this: Aaron Barlow
        /// </summary>
        /// <returns></returns>
        public IActionResult Developer()
        {
            return View();
        }

        /// <summary>
        /// When an error occurs, display a pretty error view.
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
