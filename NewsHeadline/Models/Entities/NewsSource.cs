using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsHeadline.Models.Entities
{
    /// <summary>
    /// News Source model.
    /// Mapped to the News Source model as defined in the application database context
    /// </summary>
    public class NewsSource
    {
        public int Id { get; set; }
        public int UserSettingNewsCategoryId { get; set; }
        public UserSettingNewsCategory UserSettingNewsCategory { get; set; }
        public string SourceId { get; set; } // NewsAPI source ID
    }
}
