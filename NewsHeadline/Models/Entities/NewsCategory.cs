using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsHeadline.Models.Entities
{
    /// <summary>
    /// News category model.
    /// Mapped to the NewsCategory table as defined in application database context
    /// </summary>
    public class NewsCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserSettingNewsCategory> UserSettingNewsCategories { get; set; }

        public NewsCategory()
        {
            UserSettingNewsCategories = new List<UserSettingNewsCategory>();
        }
    }
}
