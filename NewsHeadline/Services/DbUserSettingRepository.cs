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
    /// Implementation of the user setting repository interface.
    /// </summary>
    public class DbUserSettingRepository : IUserSettingRepository
    {

        private ApplicationDbContext _applicationdb;
        private UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Constructor to bring in the application db context and the user manager
        /// </summary>
        /// <param name="applicationdb"></param>
        /// <param name="userManager"></param>
        public DbUserSettingRepository(ApplicationDbContext applicationdb, UserManager<IdentityUser> userManager)
        {
            _applicationdb = applicationdb;
            _userManager = userManager;
        }

        /// <summary>
        /// Read in an identity user username and find the
        /// identity user object from the username.
        /// Then, find the associated user setting and return
        /// it along with all associations.
        /// The first time this is called, the identity user
        /// does not have an associated user setting, so it is created.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>UserSetting</returns>
        public UserSetting Read(string userName)
        {
            UserSetting user = null;
            IdentityUser identityUser = _applicationdb.Users.FirstOrDefault(u => u.UserName == userName);

            // When the identity user is not found
            if (identityUser != null)
            {
                // Get the user setting
                user = _applicationdb.UserSettings.Include(u => u.UserSettingNewsCategories).FirstOrDefault(u => u.IdentityUserId == identityUser.Id);
                // If the user is not found
                if(user == null)
                {
                    // Create the user profile settings
                    user = new UserSetting
                    {
                        Id = 0,
                        IdentityUserId = identityUser.Id,
                        User = identityUser,
                        FirstName = "",
                        LastName = "",
                        Country = "us",
                        Language = "en"
                    };
                    
                    // Create the default general news category
                    var userNewsCategory = new UserSettingNewsCategory
                    {
                        Id = 0,
                        NewsCategory = _applicationdb.NewsCategories.Include(u => u.UserSettingNewsCategories).FirstOrDefault(n => n.Name == "general"),
                        UserSetting = user
                    };

                    // Add the news category
                    user.UserSettingNewsCategories.Add(userNewsCategory);
                    
                    // Save all the additions
                    _applicationdb.UserSettings.Add(user);
                    _applicationdb.SaveChanges();
                }
            }

            // When the user is found
            if(user != null)
            {
                // Iterate over each news settings
                foreach (var settingNews in user.UserSettingNewsCategories)
                {
                    // Load all associations
                    _applicationdb.Entry(settingNews).Reference("NewsCategory").Load();
                    _applicationdb.Entry(settingNews).Collection("NewsSources").Load();
                }
            }

            return user;
        }

        /// <summary>
        /// Update the user setting profile given the identity user's username.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userSetting"></param>
        /// <param name="newsCategoryNames"></param>
        public void Update(string userName, UserSetting userSetting, ICollection<string> newsCategoryNames)
        {
            // Find the user setting
            var oldUserSetting = Read(userName);
            // When the setting is found
            if (oldUserSetting != null)
            {
                // And when at least one categories are provided
                if (newsCategoryNames != null)
                {
                    // Iterate over each news category
                    foreach (var news in newsCategoryNames)
                    {
                        // Create the news category
                        var userNewsCategory = new UserSettingNewsCategory
                        {
                            Id = 0,
                            NewsCategory = _applicationdb.NewsCategories.FirstOrDefault(n => n.Name == news),
                            UserSetting = userSetting
                        };
                        oldUserSetting.UserSettingNewsCategories.Add(userNewsCategory);
                    }
                }
                // Update all old settings that can be changed
                oldUserSetting.FirstName = userSetting.FirstName;
                oldUserSetting.LastName = userSetting.LastName;
                _applicationdb.SaveChanges();
            }
        }
        
    }
}
