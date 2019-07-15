using Microsoft.AspNetCore.Identity;
using NewsHeadline.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsHeadline.Services
{
    /// <summary>
    /// Intialize the application with data
    /// </summary>
    public class Initializer
    {
        private ApplicationDbContext _applicationdb;
        private INewsCategoryRepository _newsCategory;

        /// <summary>
        /// Constructor to get and set database context and news category interface
        /// </summary>
        /// <param name="applicationdb"></param>
        /// <param name="newsCategory"></param>
        public Initializer(ApplicationDbContext applicationdb, INewsCategoryRepository newsCategory)
        {
            _applicationdb = applicationdb;
            _newsCategory = newsCategory;
        }
        
        /// <summary>
        /// Create business, entertainment, general, health, science, sports, and technology categories
        /// Which match the NewsAPI categories specified
        /// </summary>
        public void SeedCategories()
        {
            _applicationdb.Database.EnsureCreated();

            string[] allCategories = { "business", "entertainment", "general", "health", "science", "sports", "technology" };

            // Iterate over possible cateogires
            foreach(var category in allCategories)
            {
                // When the category does not exist
                if(!_applicationdb.NewsCategories.Any(c => c.Name == category))
                {
                    // Create the category
                    _newsCategory.CreateUserSettingNewsCategory(category);
                }
            }
        }

    }
}
