using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsHeadline.Models.Entities
{
    /// <summary>
    /// User Setting model.
    /// Mapped to the UserSetting model, as defined in the application database context.
    /// </summary>
    public class UserSetting
    {
        public int Id { get; set; }
        public string IdentityUserId { get; set; }
        public IdentityUser User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        
        public ICollection<UserSettingNewsCategory> UserSettingNewsCategories { get; set; }

        public UserSetting()
        {
            UserSettingNewsCategories = new List<UserSettingNewsCategory>();
        }
    }
}
