using NewsHeadline.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsHeadline.Models.ViewModels
{
    /// <summary>
    /// View model for adding/removing sources
    /// </summary>
    public class AddSourceVM
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public UserSettingNewsCategory UserSettingNewsCategory { get; set; }
        [Display(Name = "News Sources")]
        public ICollection<NewsSource> AllNewSources { get; set; }
        public ICollection<NewsSource> ExistingNewSources { get; set; }
        public ICollection<string> NewsSources { get; set; } // nbc, cnn
    }
}
