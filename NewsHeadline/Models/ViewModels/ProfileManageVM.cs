using Microsoft.AspNetCore.Identity;
using NewsHeadline.Models.Entities;
using NewsHeadline.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsHeadline.Models.ViewModels
{
    /// <summary>
    /// View model for managing a user's profile
    /// </summary>
    public class ProfileManageVM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public ICollection<UserSettingNewsCategory> UserSettingNewsCategories { get; set; }
        public ICollection<NewsCategory> SubtractedNewsCategoriesOptions { get; set; }
        [Display(Name = "News Categories to Add")]
        public ICollection<string> NewsCategoryNames { get; set; }

        public UserSetting GetInstance(UserSetting currentUser)
        {
            return new UserSetting
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Language = Language,
                Country = Country,
                User = currentUser.User,
                IdentityUserId = currentUser.User.Id
            };
        }

    }
}
