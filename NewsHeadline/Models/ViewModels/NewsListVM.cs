using NewsHeadline.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsHeadline.Models.ViewModels
{
    /// <summary>
    /// View model for the news list that gets displayed on the home page
    /// </summary>
    public class NewsListVM
    {
        public ICollection<UserSettingNewsCategory> UserSettingNewsCategories { get; set; }
        public string Country { get; set; }
        public string CategorySources { get; set; }
    }
}
