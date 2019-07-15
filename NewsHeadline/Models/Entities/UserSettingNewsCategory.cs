using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsHeadline.Models.Entities
{
    /// <summary>
    /// UserSettingNewsCategory model.
    /// Mapped to the UserSettingNewsCategory model, as defined in the application database context.
    /// </summary>
    public class UserSettingNewsCategory
    {
        public int Id { get; set; }
        public int UserSettingId { get; set; }
        public UserSetting UserSetting { get; set; }
        public int NewsCategoryId { get; set; }
        public NewsCategory NewsCategory { get; set; }
        public ICollection<NewsSource> NewsSources { get; set; }

        public UserSettingNewsCategory()
        {
            NewsSources = new List<NewsSource>();
        }
    }
}
