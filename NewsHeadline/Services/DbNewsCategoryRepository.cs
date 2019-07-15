using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsHeadline.Data;
using NewsHeadline.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsHeadline.Services
{
    /// <summary>
    /// Impliments the news category repository interface.
    /// </summary>
    public class DbNewsCategoryRepository : INewsCategoryRepository
    {
        private ApplicationDbContext _db;

        /// <summary>
        /// Constructor that gets and sets the application db context
        /// </summary>
        /// <param name="db"></param>
        public DbNewsCategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Create a new user setting news category given a string
        /// </summary>
        /// <param name="name"></param>
        public void CreateUserSettingNewsCategory(string name)
        {
            // Instiantiate a new news category
            var newsCategory = new NewsCategory
            {
                Name = name
            };
            // Then, add and create it
            _db.Add(newsCategory);
            _db.SaveChanges();
        }

        /// <summary>
        /// Create a news source given a source name and a category
        /// </summary>
        /// <param name="name"></param>
        /// <param name="category"></param>
        public void CreateNewsSource(string name, UserSettingNewsCategory category)
        {
            // Instantiate a new news source
            var source = new NewsSource
            {
                Id = 0,
                SourceId = name,
                UserSettingNewsCategory = category,
                UserSettingNewsCategoryId = category.Id
            };

            // Add the source to the news sources
            _db.NewsSources.Add(source);
            // Also add the news source to the category
            category.NewsSources.Add(source);
            // Save the changes
            _db.SaveChanges();
        }

        /// <summary>
        /// Delete a user setting news category associative entity.
        /// Take a news category id and a user setting, and make it so.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userSetting"></param>
        public void Delete(int id, UserSetting userSetting)
        {
            // Find the news cateogry
            var settingNewsCat = userSetting.UserSettingNewsCategories.FirstOrDefault(news => news.Id == id);

            // When the news category is found
            if(settingNewsCat != null)
            {
                // Delete the news category from the user's list
                _db.UserSettingNewsCategories.Remove(settingNewsCat);
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// Find a user setting given an id.
        /// </summary>
        /// <param name="userSettingId"></param>
        /// <returns></returns>
        public UserSetting Read(int userSettingId)
        {
            // Find a user setting given the user setting ID.
            var user =_db.UserSettings.Include(u => u.UserSettingNewsCategories).FirstOrDefault(u => u.Id == userSettingId);

            // Iterate over the news category assocation
            foreach(var settingNews in user.UserSettingNewsCategories)
            {
                // Eager load the user setting news category associations.
                _db.Entry(settingNews).Reference("NewsCategory").Load();
                _db.Entry(settingNews).Collection("NewsSources").Load();
            }
            return user;
        }

        /// <summary>
        /// Find all news categories
        /// </summary>
        /// <returns></returns>
        public IQueryable<NewsCategory> ReadAll()
        {
            return _db.NewsCategories.Include(nc => nc.UserSettingNewsCategories);
        }

        /// <summary>
        /// Find a user setting news category given an ID.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public UserSettingNewsCategory ReadUserSettingNewsCategory(int categoryId)
        {
            // Find a user setting news category then
            // eager load each association
            var category = _db.UserSettingNewsCategories
                .Include(u => u.NewsSources)
                .Include(u => u.NewsCategory)
                .Include(u => u.UserSetting)
                .FirstOrDefault(u => u.Id == categoryId);
            
            return category;
        }

        /// <summary>
        /// Delete a news source given a source ID
        /// And a user setting object.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        public void DeleteNewsSource(int id, UserSetting user)
        {
            // Search for the user, and eager load the associations
            var userAssoc = Read(user.Id);
            NewsSource connectedSource = null;
            UserSettingNewsCategory connectedCategory = null;
            
            // Iterate over each category
            foreach(var category in userAssoc.UserSettingNewsCategories)
            {
                // Find the news category
                connectedCategory = category;
                var firstNewsSource = category.NewsSources.Where(s => s.Id == id);

                // When the news source is found
                if(firstNewsSource != null && firstNewsSource.Count() >= 1)
                {
                    // Add it to the list of sources that have been found connected to the user's category
                    connectedSource = firstNewsSource.First();
                }
            }

            // When the user, source, and category are identified to exist
            if (connectedSource != null && connectedCategory != null && userAssoc != null)
            {
                // Delete the news source
                _db.NewsSources.Remove(connectedSource);
                connectedCategory.NewsSources.Remove(connectedSource);
                _db.SaveChanges();
            }
        }
    }
}
